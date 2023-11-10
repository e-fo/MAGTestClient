using UnityEngine;

public struct PopIslandRule: IRuleTileTap
{
    public async void Execute(Vector2Int position, Puzzle puzzle)
    {
        var tile = puzzle.Table[position.x, position.y];
        var go = puzzle.TilesRefComponents[tile.GameObjectInstanceId];

        Debug.Log($"You tapped on tile with position: {go.Transform.position}");
        var typeGrid = PuzzleLogic.GetTypeGrid(puzzle.Table);
        var islandIndicies = PuzzleLogic.GetIslandIndices(typeGrid, position);

        if(islandIndicies.Length > 1)
        {
            {
                TileStateRef[] shouldDestroy = new TileStateRef[islandIndicies.Length];
                for(int i=0; i< islandIndicies.Length; ++i) 
                {
                    var idx = islandIndicies[i];
                    int instanceId = puzzle.Table[idx.x, idx.y].GameObjectInstanceId;
                    shouldDestroy[i] = puzzle.TilesRefComponents[instanceId]; 
                }

                await PuzzlePresentation.BatchDestroy(shouldDestroy);
                PuzzleLogic.BatchDestroyTilesUtil(ref puzzle, islandIndicies);
            }
        }
    }
}