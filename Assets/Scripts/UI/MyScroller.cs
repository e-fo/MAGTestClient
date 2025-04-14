using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.ScrollRect;

public enum ScrollDirection
{
    Vertical,
    Horizontal,
}

public enum MovementDirection
{
    Left,
    Right,
    Up,
    Down,
}

public class MyScroller : UIBehaviour, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Serializable]
    private class Snap
    {
        public bool Enable;
        public float Duration;
        public float VelocityThreshold;
        public Ease Easing;
    }

    public event Action<float> OnValueChanged;
    public event Action<int> OnSelectionChanged;
    public ScrollDirection ScrollDirection = ScrollDirection.Vertical;
    public MovementType MovementType = MovementType.Elastic;
    public float Elasticity = 0.1f;
    public float ScrollSensitivity = 1f;
    public bool Inertia = true;
    public float DecelerationRate = 0.03f;
    public bool Draggable = true;
    [NonSerialized] public int TotalCount;
    private Vector2 _beginDragPosition;
    private float _startPosition;
    private float _velocity;
    private bool _dragging;
    private Tween _autoScrollTween;

    [SerializeField] private RectTransform _viewport = default;
    public float ViewportSize => ScrollDirection == ScrollDirection.Horizontal
    ? _viewport.rect.size.x
    : _viewport.rect.size.y;

    [SerializeField]
    Snap _snap = new Snap
    {
        Enable = true,
        VelocityThreshold = 0.5f,
        Duration = 0.3f,
        Easing = Ease.InOutCubic
    };
    public bool SnapEnabled
    {
        get => _snap.Enable;
        set => _snap.Enable = value;
    }

    public float CurrentPosition { get; private set; }

    public void ScrollTo(float endPos, float duration, Ease ease)
    {
        if (duration <= 0f)
        {
            CurrentPosition = ArrayUtil.CircularPosition(endPos, TotalCount);
            ResetAutoScrolling();
            return;
        }

        if(_autoScrollTween.IsActive()) _autoScrollTween.Kill();

        float currentScrollPos = CurrentPosition;

        //TODO: this part generates garabage and need an improvement.
        _autoScrollTween = DOTween.To(()=>currentScrollPos, f=>{
            CurrentPosition = ArrayUtil.CircularPosition(f, TotalCount);
            OnValueChanged?.Invoke(CurrentPosition);
            currentScrollPos = f;
        },
        endPos,
        duration
        ).SetEase(ease).OnComplete(()=>{
            if(_snap.Enable)
            {
                CurrentPosition = Mathf.Clamp(Mathf.RoundToInt(CurrentPosition), 0, TotalCount - 1);
                OnValueChanged?.Invoke(CurrentPosition);
                OnSelectionChanged?.Invoke(Mathf.RoundToInt(ArrayUtil.CircularPosition(endPos, TotalCount)));
            }
        });
        
        _velocity = 0;
        _startPosition = CurrentPosition;
    }

    public void JumpTo(int index)
    {
        if (index < 0 || index > TotalCount - 1)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        CurrentPosition = index;
        ResetAutoScrolling();
        OnSelectionChanged?.Invoke(index);
    }

    private void ResetAutoScrolling()
    {
        if(_autoScrollTween.IsActive())
        {
            _autoScrollTween.Kill();
        }
        _velocity = 0;
    }

    private float GetOffset(float pos)
    {
        float ret = 0;

        if (MovementType == MovementType.Unrestricted)
        {
            ret = 0f;
        }
        else if (pos < 0f)
        {
            ret = -pos;
        }
        else if (pos > TotalCount - 1)
        {
            ret = TotalCount - 1 - pos;
        }
        return ret;
    }

    public float Magnet(float overScrolling, float viewSize) =>
        (1 - 1 / (Mathf.Abs(overScrolling) * 0.55f / viewSize + 1)) * viewSize * Mathf.Sign(overScrolling);

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (Draggable)
        {
            ResetAutoScrolling();
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (Draggable && _snap.Enable)
        {
            //ScrollTo(Mathf.RoundToInt(CurrentPosition), _snap.Duration, _snap.Easing);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (Draggable)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _viewport,
                eventData.position,
                eventData.pressEventCamera,
                out _beginDragPosition
                );

            _startPosition = CurrentPosition;
            _dragging = true;
            ResetAutoScrolling();
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        bool clickInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _viewport,
            eventData.position,
            eventData.pressEventCamera,
            out var dragPosition);

        if (Draggable && clickInside)
        {
            Vector2 delta = dragPosition - _beginDragPosition;
            float pos = (ScrollDirection == ScrollDirection.Horizontal ? -delta.x : delta.y)
               / ViewportSize
               * ScrollSensitivity
               + _startPosition;

            float offset = GetOffset(pos);
            pos += GetOffset(pos);


            if(MovementType == MovementType.Elastic)
            {
                pos -= Magnet(offset, ScrollSensitivity);
            }

            CurrentPosition = pos;
            OnValueChanged?.Invoke(CurrentPosition);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if(Draggable)
        {
            _dragging = false;
        }
    }

    void Update()
    {
        var deltaTime = Time.unscaledDeltaTime;
        var offset = GetOffset(CurrentPosition);

        if( !_autoScrollTween.IsActive() )
        {
            float pos = CurrentPosition;
            if(!_dragging && (!Mathf.Approximately(_velocity, 0) || !Mathf.Approximately(offset, 0)))
            {
                if (Inertia)
                {
                    if (MovementType == MovementType.Elastic && !Mathf.Approximately(offset, 0f))
                    {
                        pos = Mathf.SmoothDamp(CurrentPosition, CurrentPosition + offset, ref _velocity,
                            Elasticity, Mathf.Infinity, deltaTime);

                        if (Mathf.Abs(_velocity) < 0.01f)
                        {
                            pos = Mathf.Clamp(Mathf.RoundToInt(pos), 0, TotalCount - 1);
                            _velocity = 0f;
                        }

                    } else
                    {
                        _velocity *= Mathf.Pow(DecelerationRate, deltaTime);
                        pos += _velocity * deltaTime;
                        if (MovementType == MovementType.Elastic)
                        {
                            if (offset != 0f)
                            {
                                pos -= Magnet(offset, ScrollSensitivity);
                            }
                        }

                        if (_snap.Enable && Mathf.Abs(_velocity) < _snap.VelocityThreshold)
                        {
                            ScrollTo(Mathf.RoundToInt(CurrentPosition), _snap.Duration, _snap.Easing);
                        }
                    }
                } 
                else
                {
                    _velocity = 0;
                }

                if (MovementType == MovementType.Clamped)
                {
                    offset = GetOffset(pos);
                    pos += offset;
                    if (Mathf.Approximately(pos, 0f) || Mathf.Approximately(pos, TotalCount - 1f))
                    {
                        _velocity = 0f;
                        OnSelectionChanged?.Invoke(Mathf.RoundToInt(pos));
                    }
                }
                CurrentPosition = pos;
                OnValueChanged?.Invoke(CurrentPosition);
            }

            if (Inertia && _dragging)
            {
                var newVelocity = (CurrentPosition - _prevPosition) / deltaTime;
                _velocity = Mathf.Lerp(_velocity, newVelocity, deltaTime * 10f);
            }
            _prevPosition = CurrentPosition;
        }
    }
    float _prevPosition;
}