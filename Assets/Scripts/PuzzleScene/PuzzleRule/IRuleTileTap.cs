using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// All classes which implement this interface automatically are called by PuzzleState.RunGame() class.
/// </summary>
public interface IRuleTileTap
{
    /// <summary>
    /// PuzzleState.RunGame() calls Execute() method when selected tile type exists in this field
    /// </summary>
    public TileBaseType[] AcceptedBaseTypes {get;}

    /// <param name="position">stores tapped position</param>
    /// <param name="puzzle">stores all puzzle data</param>
    public UniTask Execute(Vector2Int position, PuzzleSceneData sceneData);
}