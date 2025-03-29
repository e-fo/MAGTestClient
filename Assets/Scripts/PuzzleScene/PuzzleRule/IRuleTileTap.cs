using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// All classes which implement this interface automatically are called by PuzzleInputHandler class.
/// </summary>
public interface IRuleTileTap
{
    /// <summary>
    /// PuzzleInputHandler calls Execute() method when selected tile type exists in this field
    /// </summary>
    public TileBaseType[] AcceptedBaseTypes {get;}

    /// <param name="position">stores tapped position</param>
    /// <param name="puzzle">stores all puzzle data</param>
    public UniTask Execute(Vector2Int position, Puzzle puzzle);
}