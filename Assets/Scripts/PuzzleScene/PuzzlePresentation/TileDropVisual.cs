using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;

public static partial class PuzzlePresentation
{
    public static async Task TileDropVisual(Puzzle puzzleState, int[,] dropMap)
    {
        int rows = dropMap.GetLength(0);
        int cols = dropMap.GetLength(1);

        List<Task> dropsAnim = new();
        for(int i=0; i<rows; ++i) 
            for(int j=0; j<cols; ++j)
            {
                int drop = dropMap[i,j];
                if (drop > 0)
                {
                    int id = puzzleState.Table[i,j].GameObjectInstanceId;
                    var t = puzzleState.TilesRefComponents[id].Transform;
                    
                    dropsAnim.Add(t.DOMoveY(t.position.y - drop, 0.2f).Play().AsyncWaitForCompletion());
                }
            }
        await Task.WhenAll(dropsAnim);
    }
}