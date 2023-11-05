using UnityEngine;

public class PuzzleInputHandler : MonoBehaviour
{
    private Puzzle _puzzel;
    IRuleTileTap[] tapRules = null;

    private void OnEnable()
    {
        _puzzel= GetComponent<Puzzle>();


    }

    public void OnTapHandler(Vector2Int pos)
    {

    }
}