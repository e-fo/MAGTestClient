using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static partial class PuzzlePresentation
{
    public static async Task RefillDropVisual(Puzzle puzzleState, int[,] refillGrid)
    {
        int rows = refillGrid.GetLength(0);
        int cols = refillGrid.GetLength(1);

        var idGrid = PuzzleLogic.GetIdGrid(puzzleState.Table);
        Vector2Int indexOf(int instanceId)
        {
            for(int i=0; i<rows; ++i) 
                for(int j=0; j<cols; ++j)
                {
                    if(idGrid[i,j] == instanceId)
                        return new Vector2Int(i,j);
                }
            return new Vector2Int(-1,-1);
        }

        int[] colsDrop = new int[cols];
        for(int j=0; j<colsDrop.Length; ++j)
        {
            int newTileInstanceId = refillGrid[0,j];
            if(TileStateValue.Empty.SOEnumTypeInstanceId != newTileInstanceId)
            {
                var idx = indexOf(newTileInstanceId);
                colsDrop[j] = rows - idx.y;
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
                    
                    dropAnims.Add(t.DOMoveY(t.position.y - colsDrop[j], 0.2f).Play().AsyncWaitForCompletion());
                }
            }

        await Task.WhenAll(dropAnims);
    }
}