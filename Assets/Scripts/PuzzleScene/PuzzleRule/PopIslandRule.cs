using UnityEngine;

public struct PopIslandRule: IRuleTileTap
{
    public async void Execute(Vector2Int position, Puzzle puzzle)
    {
        var tile = puzzle.Table[position.x, position.y];
        var go = puzzle.TilesRefComponents[tile.GameObjectInstanceId];

        Debug.Log($"You tapped on tile with position: {go.Transform.position}");

        var islandIndicies = PuzzleLogic.GetIslandIndices(
            PuzzleLogic.GetTypeGrid(puzzle.Table), 
            position);

        if(islandIndicies.Length > 1)
        {
            await PuzzlePresentation.BatchDestroy(puzzle, islandIndicies);
            PuzzleLogic.BatchDestroyTilesUtil(puzzle, islandIndicies);

            var dropMap = PuzzleLogic.CalculateTilesDrop(PuzzleLogic.GetIdGrid(puzzle.Table));
            await PuzzlePresentation.TileDropVisual(puzzle, dropMap);
            PuzzleLogic.ApplyTilesDrop(puzzle, dropMap);

            var refillGrid = PuzzleLogic.GenerateRefillGrid(puzzle);
            //await PuzzlePresentation.RefillDropVisual(puzzle, refillGrid);
        }
    }
}