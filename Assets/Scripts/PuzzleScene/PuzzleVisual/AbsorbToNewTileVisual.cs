using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/AbsorbToNewTileVisual", order = 2)]
public class AbsorbToNewTileVisual : VisualConfigBase
{
    [SerializeField] 
    float _islandAbsorbDuration = 0.2f;            public float IslandAbsorbDuration => _islandAbsorbDuration;
    [SerializeField] 
    float _delayUntilNewTileStartAppearing = 0.1f; public float DelayUntilNewTileStartAppearing => _delayUntilNewTileStartAppearing;
    [SerializeField] 
    float _newTileAppearDuration = 0.2f;           public float NewTileAppearDuration => _newTileAppearDuration;
}

public static partial class PuzzlePresentation
{
    public static async Task AbsorbToNewTile(Puzzle puzzleState, int[,] destroyMap, TileStateRef newTile)
    {
        var visualConf = puzzleState.VisualConfigs.Get<AbsorbToNewTileVisual>();

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
                    tweens.Add(t.DOMove(pos, visualConf.IslandAbsorbDuration).Play().AsyncWaitForCompletion());
                    tweens.Add(r.DOFade(0,  visualConf.IslandAbsorbDuration).Play().AsyncWaitForCompletion());
                }
            }

        await Task.Delay(Mathf.RoundToInt(visualConf.DelayUntilNewTileStartAppearing*1000));

        await newTile.Renderer.DOFade(1, visualConf.NewTileAppearDuration).Play().AsyncWaitForCompletion();
    }
}