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

        var p = position - _SCROLL_OFFSET / _cellInterval;
        var firstIndex = Mathf.CeilToInt(p);
        var firstPosition = (Mathf.Ceil(p) - p) * _cellInterval;

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
                var cell = Instantiate(_cellPrefab, _cellContainer).GetComponent<Cell>();
                if (cell == null)
                {
                    throw new MissingComponentException(string.Format(
                "FancyCell<{0}, {1}> component not found in {2}.",
                typeof(Cell).FullName, typeof(Cell).FullName, _cellPrefab.name));
                }

                cell.Initialize(this);

                _pool.Add(cell);
            }
        }
    }

    private void UpdateCells(float firstPosition, int firstIndex, bool forceRefresh)
    {
        for (var i = 0; i < _pool.Count; i++)
        {
            var index = firstIndex + i;
            var pos = firstPosition + i * _cellInterval;
            int idx = ArrayUtil.CircularIndex(index, _pool.Count);
            var cell = _pool[idx];

            if (_loop)
            {
                index = ArrayUtil.CircularIndex(index, _contentList.Count);
            }

            if (index < 0 || index >= _contentList.Count || pos > 1f)
            {
                cell.Disable();
                continue;
            }

            if (forceRefresh || cell.Index != index || !cell.gameObject.activeSelf)
            {
                cell.Index = index;
                cell.Enable(_contentList[index]);
            }

            cell.UpdatePosition(pos);
        }
    }

    private void UpdateOrdering(List<Cell> cells)
    {
        //TODO: We can reduce garbage generation here.
        List<Cell> activeCells = cells.Where(c => c.gameObject.activeInHierarchy).OrderBy(c => c.CurrentPosition).ToList();

        if (activeCells.Count > 0)
        {
            int middleIndex = 0;
            for (int i = 1; i < activeCells.Count; ++i)
            {
                if (Mathf.Abs(activeCells[i].CurrentPosition - _SCROLL_OFFSET) < Mathf.Abs(activeCells[middleIndex].CurrentPosition - _SCROLL_OFFSET))
                {
                    middleIndex = i;
                }
            }

            float selectedCellPosition = activeCells[middleIndex].CurrentPosition;
            int c = activeCells.Count;
            for (var i = 0; i < activeCells.Count; ++i)
            {
                if (activeCells[i].CurrentPosition < selectedCellPosition)
                {
                    activeCells[i].transform.SetSiblingIndex(i);
                } else
                {
                    c--;
                    if (activeCells[c].CurrentPosition > selectedCellPosition)
                    {
                        activeCells[c].transform.SetAsLastSibling();
                    }
                }
            }

            activeCells[middleIndex].transform.SetAsLastSibling();
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
    float _EDITOR_CachedCellInterval, _EDITOR_CachedScrollOffset;

    void LateUpdate()
    {
        if (_EDITOR_CachedLoop != _loop ||
            _EDITOR_CachedCellInterval != _cellInterval ||
            _EDITOR_CachedScrollOffset != _SCROLL_OFFSET)
        {
            _EDITOR_CachedLoop = _loop;
            _EDITOR_CachedCellInterval = _cellInterval;
            _EDITOR_CachedScrollOffset = _SCROLL_OFFSET;

            UpdatePosition(_currentPosition, false);
        }
    }
#endif
}