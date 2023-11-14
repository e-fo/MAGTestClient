using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;

public static partial class PuzzlePresentation
{
    public static async Task AbsorbToNewTileVisual(Puzzle puzzleState, int[,] destroyMap, TileStateRef newTile)
    {
        int rows = destroyMap.GetLength(0);
        int cols = destroyMap.GetLength(1);
        var refs = puzzleState.TilesRefComponents;

        var color = newTile.Renderer.color;
        var pos = newTile.Transform.position;
        newTile.Renderer.color = new UnityEngine.Color(color.r, color.g, color.b, 0);

        List<Task> tweens = new(ArrayUtil.CountNotEqual2D(destroyMap, TileStateValue.Empty.GameObjectInstanceId) + 1);
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                int instanceId = destroyMap[i,j];
                if(instanceId != TileStateValue.Empty.GameObjectInstanceId)
                {
                    var t = refs[instanceId].Transform;
                    var r = refs[instanceId].Renderer;
                    tweens.Add(t.DOMove(pos,0.2f).Play().AsyncWaitForCompletion());
                    tweens.Add(r.DOFade(0,  0.2f).Play().AsyncWaitForCompletion());
                }
            }

        await Task.Delay(100);

        await newTile.Renderer.DOFade(1, 0.2f).Play().AsyncWaitForCompletion();
    }
}