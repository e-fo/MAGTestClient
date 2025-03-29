using UnityEngine;

public class PuzzleSceneData : MonoBehaviour
{
    [SerializeField] Puzzle _puzzleController;
    public Puzzle PuzzleController => _puzzleController;

    [SerializeField] LevelConfigList _levelList;
    public LevelConfigList LevelList => _levelList;

    [SerializeField] PuzzleUI _puzzleUI;
    public PuzzleUI PuzzleUI => _puzzleUI;
}