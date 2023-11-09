using UnityEngine;

public static partial class PuzzleUtil
{
    public static void BatchDestroyTilesUtil(ref Puzzle puzzleState, in Vector2Int[] shouldDestroyIndices)
    {
        foreach(var idx in shouldDestroyIndices) {
            int instanceId = puzzleState.Table[idx.x, idx.y].GameObjectInstanceId;
            puzzleState.Table[idx.x, idx.y] = TileStateValue.Empty;
            Object.Destroy(puzzleState.TilesRefComponents[instanceId].Transform.gameObject);
            puzzleState.TilesRefComponents.Remove(instanceId);
        }
    }
}