using UnityEngine;

public struct PopIslandRule: IRuleTileTap
{
    public void Execute(Vector2Int position, Puzzle puzzle)
    {
        var tile = puzzle.Table[position.x, position.y];
        var go = puzzle.TilesRefComponents[tile.GameObjectInstanceId];

        Debug.Log($"You tapped on tile with position: {go.Transform.position}");
        var typeGrid = PuzzleLogic.GetTypeGrid(puzzle.Table);
        var islandIndicies = PuzzleLogic.GetIslandIndices(typeGrid, position);

        if(islandIndicies.Length > 1)
        {
            PuzzleLogic.BatchDestroyTilesUtil(ref puzzle, islandIndicies);
        }
    }
}