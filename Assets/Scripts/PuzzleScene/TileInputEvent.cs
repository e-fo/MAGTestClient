using UnityEngine;
using UnityEngine.Events;

public class TileInputEvent : MonoBehaviour
{
    public UnityEvent<Vector2Int> OnTileTapped;

    private void OnMouseDown()
    {
        OnTileTapped?.Invoke(new Vector2Int(
            Mathf.RoundToInt(transform.position.x), 
            Mathf.RoundToInt(transform.position.y)
            ));
    }
}