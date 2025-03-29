using UnityEngine;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField] Button _restartButton;
    public Button RestartButton => _restartButton;

    [SerializeField] Button _lvlSelectionButton;
    public Button LvlSelectionButton => _lvlSelectionButton;
}