using UnityEngine;

/// <summary>
/// All classes which implement this interface automatically are called by PuzzleInputHandler class.
/// </summary>
public interface IRuleTileTap
{

    /// <param name="position">stores tapped position</param>
    /// <param name="puzzle">stores all puzzle data</param>
    public void Execute(Vector2Int position, Puzzle puzzle);
}