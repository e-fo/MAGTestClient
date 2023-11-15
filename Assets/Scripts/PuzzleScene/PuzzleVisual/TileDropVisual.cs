using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/TileDropVisual", order = 2)]
public class TileDropVisual: VisualConfigBase
{
    [SerializeField] 
    float _dropDuration = 0.2f; public float DropDuration => _dropDuration;
}


public static partial class PuzzlePresentation
{
    public static async Task TileDropVisual(Puzzle puzzleState, int[,] dropMap)
    {
        var visualConf = puzzleState.VisualConfigs.Get<TileDropVisual>();

        int rows = dropMap.GetLength(0);
        int cols = dropMap.GetLength(1);

        List<Task> dropsAnim = new();
        for(int i=0; i<rows; ++i) 
            for(int j=0; j<cols; ++j)
            {
                int drop = dropMap[i,j];
                if (drop > 0)
                {
                    int id = puzzleState.Grid[i,j].GameObjectInstanceId;
                    var t = puzzleState.TilesRefComponents[id].Transform;
                    
                    dropsAnim.Add(t.DOMoveY(t.position.y - drop, visualConf.DropDuration)
                        .Play()
                        .AsyncWaitForCompletion());
                }
            }
        await Task.WhenAll(dropsAnim);
    }
}