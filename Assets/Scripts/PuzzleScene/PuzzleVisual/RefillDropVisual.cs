using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public static partial class PuzzlePresentation
{
    public static async Task RefillDropVisual(Puzzle puzzleState, int[,] refillGrid)
    {
        int rows = refillGrid.GetLength(0);
        int cols = refillGrid.GetLength(1);

        int deepestEmptyCell = 0;
        for (int i=0; i<rows; ++i)
            for (int j=0; j<cols; ++j)
            {
                if(
                    refillGrid[i,j] != TileStateValue.Empty.GameObjectInstanceId &&
                    deepestEmptyCell < j)
                {
                    deepestEmptyCell = j;
                }
            }

        var refDict = puzzleState.TilesRefComponents;
        List<Task> dropAnims = new();
        for(int i=0; i<rows; ++i) 
            for(int j=0; j<cols; ++j)
            {
                int id = refillGrid[i,j];
                if (TileStateValue.Empty.SOEnumTypeInstanceId != id)
                {
                    var t = refDict[id].Transform;
                    t.position = new Vector3(i,j+deepestEmptyCell,0);
                    dropAnims.Add(t.DOMoveY(t.position.y - deepestEmptyCell, 0.2f).Play().AsyncWaitForCompletion());
                }
            }

        await Task.WhenAll(dropAnims);
    }
}