using System.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/BatchDestroyVisual", order = 2)]
public class BatchDestroyVisual: VisualConfigBase
{
    [SerializeField] 
    float _fadeOutDuration = 0.2f; public float FadeOutDuration => _fadeOutDuration;
}

public static partial class PuzzlePresentation
{
    public static async Task BatchDestroyVisual(Puzzle puzzleState, int[,] destroyMap)
    {
        var visualConf = puzzleState.VisualConfigs.Get<BatchDestroyVisual>();

        int rows = destroyMap.GetLength(0);
        int cols = destroyMap.GetLength(1);
        var refs = puzzleState.TilesRefComponents;

        List<Task> tweens = new(ArrayUtil.CountNotEqual2D(destroyMap, TileStateValue.Empty.GameObjectInstanceId));
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                int instanceId = destroyMap[i,j];
                if(instanceId != TileStateValue.Empty.GameObjectInstanceId)
                {
                    var r = refs[instanceId].Renderer;
                    tweens.Add(r.DOFade(0, visualConf.FadeOutDuration).Play().AsyncWaitForCompletion());
                }
            }
        await Task.WhenAll(tweens);
    }
}