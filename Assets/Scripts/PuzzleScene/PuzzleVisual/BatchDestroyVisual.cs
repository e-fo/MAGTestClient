using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public static partial class PuzzlePresentation
{
    public static async Task BatchDestroy(Puzzle puzzleState, Vector2Int[] islandIndices)
    {
        TileStateRef[] shouldDestroy = new TileStateRef[islandIndices.Length];
        for(int i=0; i< islandIndices.Length; ++i) 
        {
            var idx = islandIndices[i];
            int instanceId = puzzleState.Table[idx.x, idx.y].GameObjectInstanceId;
            shouldDestroy[i] = puzzleState.TilesRefComponents[instanceId]; 
        }
        List<Task> tweens = new(shouldDestroy.Length);

        foreach(var t in shouldDestroy) {
            var r = t.Transform.GetComponent<SpriteRenderer>();
            var tween = r.DOFade(0, 0.2f);
            tweens.Add(tween.Play().AsyncWaitForCompletion());
        }

        await Task.WhenAll(tweens);
    }
}