using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;

public static partial class PuzzlePresentation
{
    public static async Task TileShakeVisual(Puzzle puzzleState, Vector2Int idx)
    {
        int instanceId = puzzleState.Table[idx.x, idx.y].GameObjectInstanceId;
        var t = puzzleState.TilesRefComponents[instanceId].Transform;
        await t.DOShakeRotation(0.5f, 30).Play().AsyncWaitForCompletion();
    }
}