/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CellData:ICellData
{
    public CellData(string str)
    {
        text = str;
    }
    public string text;
}

class CaroaselUIExample : MonoBehaviour
{
    [SerializeField] AnimationScrollRect scrollView = default;
    [SerializeField] Button prevCellButton = default;
    [SerializeField] Button nextCellButton = default;
    [SerializeField] Text selectedItemInfo = default;

    void Start()
    {
        prevCellButton.onClick.AddListener(()=>scrollView.SelectCell(scrollView.SelectedIndex - 1));
        nextCellButton.onClick.AddListener(()=>scrollView.SelectCell(scrollView.SelectedIndex + 1));
        //scrollView.OnSelectionChanged(OnSelectionChanged);

        var items = Enumerable.Range(0, 20)
            .Select(i => new CellData($"Cell {i}"))
            .Cast<ICellData>().ToList();

        scrollView.UpdateData(items);
        scrollView.SelectCell(0);
    }

    void OnSelectionChanged(int index)
    {
        selectedItemInfo.text = $"Selected item info: index {index}";
    }
}
