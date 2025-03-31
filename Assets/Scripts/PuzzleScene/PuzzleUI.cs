using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Linq;

public class PuzzleUI : MonoBehaviour
{
    private List<ValueTuple<LevelGoal, Image, TextMeshProUGUI>> GoalIndicatorList = new();

    [SerializeField] HorizontalLayoutGroup _goalIndicatorPanel;

    [SerializeField] Button _restartButton;
    public Button RestartButton => _restartButton;

    [SerializeField] Button _lvlSelectionButton;
    public Button LvlSelectionButton => _lvlSelectionButton;

    [SerializeField] Image _goalIndicatorPrefab;

    [SerializeField] TextMeshProUGUI _remainMovesTxt;
    public TextMeshProUGUI RemainMovesTxt => _remainMovesTxt;

    [SerializeField] AlertUI _alertUI;
    public AlertUI AlertUI => _alertUI;

    public void SetRemainMoves(int remained)
    {
        RemainMovesTxt.text = remained.ToString();
    }

    public void UpdateGoal(LevelGoalState goal)
    {
        var indicator = GoalIndicatorList.First(gi =>
        {
            bool ret = false;

            ret = goal.Config.GoalType == gi.Item1.GoalType;

            if (goal.Config.GoalType == LevelGoal.Type.BaseType)
            {
                ret = goal.Config.TileBaseType == gi.Item1.TileBaseType;
            } 
            else if (goal.Config.GoalType == LevelGoal.Type.SpecificType)
            {
                ret = goal.Config.TileConfig == gi.Item1.TileConfig;
            }

            return ret;
        });
        indicator.Item3.text = goal.Remain.ToString();
    }

    public void AddGoalToIndicatorPanel(LevelGoal goal, PuzzleSceneData sceneData)
    {
        var goalObj = GameObject.Instantiate(_goalIndicatorPrefab, _goalIndicatorPanel.transform);
        var img = goalObj.GetComponentInChildren<Image>();
        var txt = goalObj.GetComponentInChildren<TextMeshProUGUI>();
        GoalIndicatorList.Add(new(goal, img, txt));

        txt.text = goal.Amount.ToString();
        switch (goal.GoalType)
        {
            case LevelGoal.Type.TypeLess:
            {
                img.sprite = sceneData.LevelList.TypeLessGoalSprite;
                img.color = Color.black;
                break;
            }
            case LevelGoal.Type.BaseType:
            {
                img.sprite = sceneData.LevelList.LvlGoalBaseTypeSpriteMap.First(spriteMap => spriteMap.Item1 == goal.TileBaseType).Item2;
                img.color = Color.black;
                break;
            }
            case LevelGoal.Type.SpecificType:
            {
                img.sprite = goal.TileConfig.Sprite;
                break;
            }
        }
    }
}