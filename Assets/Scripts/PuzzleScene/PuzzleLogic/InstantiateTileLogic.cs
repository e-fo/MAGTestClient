using UnityEngine;
using UnityEngine.Events;

public static partial class PuzzleLogic
{
    public static (TileStateValue, TileStateRef) InstantiateTile(
        in GameObject prefab, 
        in TileConfig conf, 
        in Transform parent, 
        in Vector2 pos, 
        in UnityAction<Vector2Int> onTapHandler)
    {
        GameObject tile = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        tile.GetComponent<TileInputEvent>().OnTileTapped.AddListener(onTapHandler);
        tile.transform.parent = parent;
        tile.GetComponent<SpriteRenderer>().sprite = conf.Sprite;
        int configInstanceId = conf.GetInstanceID();
        return (
            new TileStateValue(tile.GetInstanceID(), configInstanceId), 
            new TileStateRef(tile.transform));
    }
}
