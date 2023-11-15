using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleUI : MonoBehaviour
{
    [SerializeField] Button _restartButton;

    private void Awake()
    {
        _restartButton.onClick.AddListener(()=>SceneManager.LoadScene(1));
    }
}