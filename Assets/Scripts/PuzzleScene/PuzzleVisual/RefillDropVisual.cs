using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/RefillDropVisual", order = 2)]
public class RefillDropVisual: VisualConfigBase
{
    [SerializeField]
    float _dropDuration = 0.2f; public float DropDuration => _dropDuration;
}

public static partial class PuzzlePresentation
{
    public static async Task RefillDropVisual(Puzzle puzzleState, int[,] refillMap)
    {
        var visualConf = puzzleState.VisualConfigs.Get<RefillDropVisual>();

        int rows = refillMap.GetLength(0);
        int cols = refillMap.GetLength(1);

        int deepestEmptyCell = 0;
        for (int i=0; i<rows; ++i)
            for (int j=0; j<cols; ++j)
            {
                if(
                    refillMap[i,j] != TileStateValue.Empty.GameObjectInstanceId &&
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
                int id = refillMap[i,j];
                if (TileStateValue.Empty.SOEnumTypeInstanceId != id)
                {
                    var t = refDict[id].Transform;
                    t.position = new Vector3(i,j+deepestEmptyCell,0);
                    dropAnims.Add(t.DOMoveY(t.position.y - deepestEmptyCell, visualConf.DropDuration)
                        .Play()
                        .AsyncWaitForCompletion());
                }
            }

        await Task.WhenAll(dropAnims);
    }
}