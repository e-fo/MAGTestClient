using UnityEngine;

public struct PopIslandRule: IRuleTileTap
{
    public void Execute(Vector2Int position, Puzzle puzzle)
    {
        var tile = puzzle.Table[position.x, position.y];
        var go = puzzle.TilesRefComponents[tile.GameObjectInstanceId];

        Debug.Log($"You tapped on tile with position: {go.Transform.position}");
        var typeGrid = PuzzleUtil.GetTypeGrid(puzzle.Table);
        var islandIndicies = PuzzleUtil.GetIslandIndices(typeGrid, position);

        foreach(Vector2Int idx in islandIndicies)
        {
            Debug.Log($"x:{idx.x}-y:{idx.y}");
            puzzle.TilesRefComponents[puzzle.Table[idx.x, idx.y].GameObjectInstanceId]
                .Transform.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}