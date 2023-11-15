using DG.Tweening;
using UnityEngine;
using System.Threading.Tasks;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/TileShakeVisual", order = 2)]
public class ShakeTileVisual: VisualConfigBase
{
    [SerializeField]
    float _shakeStrength = 30;  public float ShakeStrength => _shakeStrength;
    [SerializeField]
    float _shakeDuration = 0.2f;public float ShakeDuration => _shakeDuration;
}

public static partial class PuzzlePresentation
{
    public static async Task TileShakeVisual(Puzzle puzzleState, Vector2Int idx)
    {
        var visualConf = puzzleState.VisualConfigs.Get<ShakeTileVisual>();

        int instanceId = puzzleState.Grid[idx.x, idx.y].GameObjectInstanceId;
        var t = puzzleState.TilesRefComponents[instanceId].Transform;
        await t.DOShakeRotation(visualConf.ShakeDuration, visualConf.ShakeStrength)
            .Play()
            .AsyncWaitForCompletion();
    }
}