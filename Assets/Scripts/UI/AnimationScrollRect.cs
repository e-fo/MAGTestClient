using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public interface ICellData { };

public class AnimationScrollRect : MonoBehaviour
{
    private const float _SCROLL_OFFSET = 0.5f;
    [SerializeField, Range(1e-2f, 1f)] private float _cellInterval = 0.2f;
    [SerializeField] private bool _loop = false;
    [SerializeField] private Transform _cellContainer = default;
    [SerializeField] MyScroller _scroller = default;
    public MyScroller Scroller => _scroller;

    private bool _initialized;
    private float _currentPosition;
    private int _selectedIndex;
    public int SelectedIndex => _selectedIndex;

    [SerializeField] private Cell _cellPrefab;
    private readonly List<Cell> _pool = new();
    private List<ICellData> _contentList { get; set; } = new();

    public void UpdateData(List<ICellData> contents)
    {
        _contentList.Clear();
        _contentList.AddRange(contents);

        UpdatePosition(_currentPosition, true);
        _scroller.TotalCount = contents.Count;
    }

    public void SelectCell(int index)
    {
        if (!_loop && (index < 0 || index >= _contentList.Count || index == _selectedIndex))
        {
            return;
        }

        int currentScrollerPosition = Mathf.FloorToInt(_scroller.CurrentPosition);
        int result = index;
        if (_loop)
        {
            int diff = Mathf.Abs(index - currentScrollerPosition);

            if (index < currentScrollerPosition)
            {
                int circularDiff = _contentList.Count - currentScrollerPosition + index;
                if (circularDiff < diff)
                {
                    result = _contentList.Count + index;
                }
            } else if (index > currentScrollerPosition)
            {
                int circularDiff = _contentList.Count - index + currentScrollerPosition;
                if (circularDiff < diff)
                {
                    result = -index;
                }
            }
        }

        UpdateSelection(ArrayUtil.CircularIndex(result, _contentList.Count));
        _scroller.ScrollTo(result, 0.35f, Ease.OutCubic);
    }

    private void UpdatePosition(float position, bool forceRefresh)
    {
        if (!_initialized)
        {
            _scroller.OnValueChanged += scrollerValue => UpdatePosition(scrollerValue, false);
            _scroller.OnSelectionChanged += UpdateSelection;
            _initialized = true;
        }

        _currentPosition = position;

        float stratPosition = position - _SCROLL_OFFSET / _cellInterval;
        int firstIndex = Mathf.CeilToInt(stratPosition);
        float firstPosition = (Mathf.Ceil(stratPosition) - stratPosition) * _cellInterval;

        ResizingPool(firstPosition);

        UpdateCells(firstPosition, firstIndex, forceRefresh);

        UpdateOrdering(_pool);
    }

    private void ResizingPool(float firstPosition)
    {
        if (firstPosition + _pool.Count * _cellInterval < 1f)
        {
            Debug.Assert(_cellPrefab != null);
            Debug.Assert(_cellContainer != null);

            var addCount = Mathf.CeilToInt((1f - firstPosition) / _cellInterval) - _pool.Count;
            for (var i = 0; i < addCount; i++)
            {
                var cell = Instantiate(_cellPrefab, _cellContainer);
                cell.Initialize(this);
                _pool.Add(cell);
            }
        }
    }

    private void UpdateCells(float firstPosition, int firstIndex, bool forceRefresh)
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            int index = firstIndex + i;
            float pos = firstPosition + i * _cellInterval;
            if (_loop)
            {
                index = ArrayUtil.CircularIndex(index, _contentList.Count);
            }

            Cell cell = _pool[ArrayUtil.CircularIndex(index, _pool.Count)];
            if (index >= 0 && index < _contentList.Count && pos <= 1f)
            {
                if (forceRefresh || cell.Index != index || !cell.gameObject.activeSelf)
                {
                    cell.Index = index;
                    cell.Enable(_contentList[index]);
                }
                cell.UpdatePosition(pos);
            } else
            {
                cell.Disable();
            }
        }
    }

    /// <summary>
    /// Stors active cells to reduce garbage collection in UpdateOrdering() method.
    /// <para>Key: Cell.transform</para>
    /// <para>Value: Cell.CurrentPosition</para>
    /// </summary>
    private readonly List<KeyValuePair<Transform, float>> _cachedActiveCells = new();
    private void UpdateOrdering(in List<Cell> cells)
    {
        cells.Sort((a, b) => a.CurrentPosition.CompareTo(b.CurrentPosition));

        _cachedActiveCells.Capacity = cells.Count;
        _cachedActiveCells.Clear();
        for(int i=0; i<cells.Count; ++i)
        {
            Cell cell = cells[i];
            if(cell.gameObject.activeInHierarchy)
            {
                _cachedActiveCells.Add(new KeyValuePair<Transform, float>(cell.transform, cell.CurrentPosition));
            }
        }
        int count = _cachedActiveCells.Count;

        if (count > 0)
        {
            int middleIndex = 0;
            for (int i = 1; i < count; ++i)
            {
                if (Mathf.Abs(_cachedActiveCells[i].Value - _SCROLL_OFFSET) < Mathf.Abs(_cachedActiveCells[middleIndex].Value - _SCROLL_OFFSET))
                {
                    middleIndex = i;
                }
            }

            float selectedCellPosition = _cachedActiveCells[middleIndex].Value;
            int cIdx = count;
            for (int i = 0; i < count; ++i)
            {
                if (_cachedActiveCells[i].Value < selectedCellPosition)
                {
                    _cachedActiveCells[i].Key.SetSiblingIndex(i);
                } else
                {
                    cIdx--;
                    if (_cachedActiveCells[cIdx].Value > selectedCellPosition)
                    {
                        _cachedActiveCells[cIdx].Key.SetAsLastSibling();
                    }
                }
            }

            _cachedActiveCells[middleIndex].Key.SetAsLastSibling();
        }
    }

    private void UpdateSelection(int index)
    {
        if (_selectedIndex == index)
        {
            return;
        }

        _selectedIndex = index;
        UpdatePosition(_currentPosition, true);

        _pool.FirstOrDefault(c => c.Index == _selectedIndex)?.transform.SetAsLastSibling();

        //onSelectionChanged?.Invoke(index);
    }

#if UNITY_EDITOR
    bool _EDITOR_CachedLoop;
    float _EDITOR_CachedCellInterval;

    void LateUpdate()
    {
        if (_EDITOR_CachedLoop != _loop || _EDITOR_CachedCellInterval != _cellInterval )
        {
            _EDITOR_CachedLoop = _loop;
            _EDITOR_CachedCellInterval = _cellInterval;

            UpdatePosition(_currentPosition, false);
        }
    }
#endif
}