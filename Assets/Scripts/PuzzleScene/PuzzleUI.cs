using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField] Button _restartButton;
    [SerializeField] Button _lvlSelectionButton;

    private void Awake()
    {
        _restartButton.onClick.AddListener(()=>SceneManager.LoadScene(1));
        _lvlSelectionButton.onClick.AddListener(()=>SceneManager.LoadScene(0));
    }
}