using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/AbsorbToBombVisual", order = 2)]
public class AbsorbToBombVisual : VisualConfigBase
{
    [SerializeField] 
    float _islandAbsorbDuration = 0.2f; public float IslandAbsorbDuration => _islandAbsorbDuration;
    [SerializeField] 
    float _delayForUpscalingBomb = 0.1f;public float DelayForUpscalingBomb => _delayForUpscalingBomb;
    [SerializeField] 
    float _bombScaleUp = 1.7f;          public float BombScaleUp => _bombScaleUp;
    [SerializeField]
    float _bombScaleDuration = 1.7f;    public float BombScaleDuration => _bombScaleDuration;
    [SerializeField]
    Ease _bombScaleUpAnimCurve;         public Ease BombScaleUpAnimCurve => _bombScaleUpAnimCurve;
}

public static partial class PuzzlePresentation
{
    public static async Task AbsorbToBomb(Puzzle puzzleState, int[,] destroyMap, Vector2Int tapPos)
    {
        var visualConf = puzzleState.VisualConfigs.Get<AbsorbToBombVisual>();

        int rows = destroyMap.GetLength(0);
        int cols = destroyMap.GetLength(1);
        var refs = puzzleState.TilesRefComponents;
        var pos = new Vector2(tapPos.x, tapPos.y);

        List<Task> tweens = new(ArrayUtil.CountNotEqual2D(destroyMap, TileStateValue.Empty.GameObjectInstanceId) + 1);
        for (int i = 0; i < rows; ++i)
            for (int j = 0; j < cols; ++j)
            {
                if(i == tapPos.x && j == tapPos.y) continue;

                int instanceId = destroyMap[i,j];
                if(instanceId != TileStateValue.Empty.GameObjectInstanceId)
                {
                    var t = refs[instanceId].Transform;
                    var r = refs[instanceId].Renderer;
                    tweens.Add(t.DOMove(pos, visualConf.IslandAbsorbDuration).Play().AsyncWaitForCompletion());
                    tweens.Add(r.DOFade(0,  visualConf.IslandAbsorbDuration).Play().AsyncWaitForCompletion());
                }
            }

        await Task.Delay(Mathf.RoundToInt(visualConf.DelayForUpscalingBomb*1000));

        var bombTransform = refs[destroyMap[tapPos.x, tapPos.y]].Transform;

        await bombTransform.DOScale(visualConf.BombScaleUp, visualConf.BombScaleDuration)
            .SetEase(visualConf.BombScaleUpAnimCurve)
            .Play()
            .AsyncWaitForCompletion();
    }
}