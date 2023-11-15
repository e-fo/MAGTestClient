using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionPanel : MonoBehaviour
{
    [SerializeField] LevelConfigList _levelList;
    [SerializeField] GridLayoutGroup _lvlButtonGridLayout;
    [SerializeField] Button _levelButtonPrefab;
    [SerializeField] Button _rightButton;
    [SerializeField] Button _leftButton;
    [SerializeField] TextMeshProUGUI _pageRangeTxt;
    [SerializeField] IntVariable _currentLevel;

    [SerializeField] int _numOfBtnInPage;

    /// <summary>
    /// [Item1] levelNum arg for button listener.
    /// </summary>
    private ValueTuple<int, TextMeshProUGUI, Button>[] _buttons = null;
    private int _startIdx = 0;

    private void Awake()
    {   
        _buttons = new ValueTuple<int, TextMeshProUGUI, Button>[_numOfBtnInPage];

        var parent = _lvlButtonGridLayout.transform;
        for(int x=0; x<_numOfBtnInPage; ++x)
        {
            var btn = Instantiate(_levelButtonPrefab, parent);
            int idx = x;
            btn.onClick.AddListener(()=>{LevelButton_OnClicked(_buttons[idx].Item1);});
            _buttons[x].Item2 = btn.GetComponentInChildren<TextMeshProUGUI>();
            _buttons[x].Item3 = btn;
        }

        _rightButton.onClick.AddListener(()=>ChangePageButton_OnClicked(next:true));
        _leftButton .onClick.AddListener(()=>ChangePageButton_OnClicked(next:false));

        _startIdx = (-1)*_numOfBtnInPage;
        ChangePageButton_OnClicked(next:true);
    }

    public void LevelButton_OnClicked(int levelNum)
    {
        _currentLevel.Value = levelNum;
        SceneManager.LoadScene(1);
    }

    public void ChangePageButton_OnClicked(bool next)
    {
        int numOfLvl = _levelList.List.Count;

        if (next) 
            _startIdx = (_startIdx + _numOfBtnInPage < numOfLvl)? _startIdx += _numOfBtnInPage:_startIdx;
        else 
            _startIdx = (_startIdx - _numOfBtnInPage < 0)? 0 :_startIdx-_numOfBtnInPage;

        int end = (_startIdx + _numOfBtnInPage < numOfLvl)? _startIdx+_numOfBtnInPage: _startIdx+(numOfLvl%_numOfBtnInPage);
        
        for(int x=0; x<_numOfBtnInPage; ++x)
        {
            if(_startIdx+x < end)
            {
                int lvl = _startIdx+x;
                _buttons[x].Item1 = lvl;
                _buttons[x].Item2.text = lvl.ToString();
                _buttons[x].Item3.interactable = true;
            }
            else
            {
                _buttons[x].Item2.text = "-";
                _buttons[x].Item3.interactable = false;
            }
        }

        _pageRangeTxt.text = $"{_startIdx}-{end}";
    }
}