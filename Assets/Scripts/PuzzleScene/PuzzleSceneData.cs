using System;
using UnityEngine;

[Serializable]
public class LevelGoalState
{
    [Header("Config --------")]
    [SerializeReference] public LevelGoal Config;

    [Header("Runtime State --------")]
    public int Remain;
}

public class PuzzleSceneData : MonoBehaviour
{
    [SerializeField] Puzzle _puzzleController;
    public Puzzle PuzzleController => _puzzleController;

    [SerializeField] LevelConfigList _levelList;
    public LevelConfigList LevelList => _levelList;

    [SerializeField] PuzzleUI _puzzleUI;
    public PuzzleUI PuzzleUI => _puzzleUI;

    [Header("Runtime Only --------")]
    public LevelGoalState[] GoalStateList;
    public int RemainMoves =0;
}