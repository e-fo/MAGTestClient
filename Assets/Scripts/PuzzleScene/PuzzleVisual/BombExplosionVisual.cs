using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/BombExplosionVisual", order = 2)]
public class BombExplosionVisual : VisualConfigBase
{
    [SerializeField] 
    float _delayBetweenExplosionLayers = 0.1f;  public float DelayBetweenExplosionLayers => _delayBetweenExplosionLayers;
    [SerializeField]
    float _destroyDuration = 0.3f;              public float DestroyDuration => _destroyDuration;
    [SerializeField] 
    float _cameraShakeDuration = 0.3f;          public float CameraShakeDuration => _cameraShakeDuration;
    [SerializeField] 
    float _cameraShakeStrength = 15;            public float CameraShakeStrength => _cameraShakeStrength;
}

public static partial class PuzzlePresentation
{
    public static async Task BombExplosion(Puzzle puzzleState, Vector2Int tapPos, int explosionLevel, int[,] destroyMap)
    {
        var visualConf = puzzleState.VisualConfigs.Get<BombExplosionVisual>();

        var grid = puzzleState.Grid;
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        var refs = puzzleState.TilesRefComponents;

        int[,] idGrid = PuzzleLogic.GetIdGrid(grid);

        {
            var bomb = refs[idGrid[tapPos.x, tapPos.y]];
            var color = bomb.Renderer.color;
            bomb.Renderer.color = new UnityEngine.Color(color.r, color.g, color.b, 0);
        }

        List<Task> tweens = new(ArrayUtil.CountNotEqual2D(destroyMap, TileStateValue.Empty.GameObjectInstanceId) + 1);
        if(explosionLevel > 1)
        {
            Camera.main.DOShakeRotation(
                visualConf.CameraShakeDuration, visualConf.CameraShakeStrength).Play();
        }

        int[,] buffer = new int[rows, cols];
        for(int lvl=1; lvl<=explosionLevel; ++lvl)
        {
            ArrayUtil.GetNeighboringMap(
                neighborMap:    ref buffer,
                array:          idGrid,
                idx:            tapPos,
                level:          lvl,
                defaultValue:   TileStateValue.Empty.GameObjectInstanceId, 
                justLastLevel:  true);

            for(int i=0; i<rows; ++i) 
                for (int j=0; j<cols; ++j) 
                {
                    if(buffer[i,j] != TileStateValue.Empty.GameObjectInstanceId)
                    {
                        var r = refs[buffer[i,j]].Renderer;
                        tweens.Add(r.DOFade(0, visualConf.DestroyDuration).Play().AsyncWaitForCompletion());
                    }
                }

            await Task.Delay((int)(visualConf.DelayBetweenExplosionLayers*1000));
        }

       await Task.WhenAll(tweens);
    }
}