using UnityEngine;

public static partial class PuzzleLogic
{
    /// <summary>
    /// Genearates another tile's grid on top of current grid
    /// </summary>
    /// <param name="puzzleState"></param>
    /// <returns>Generated tile's grid. Each cell contains instanceId of new tile which should be fill empty cell</returns>
    public static int[,] GenerateRefillGrid(Puzzle puzzleState)
    {
        int rows = puzzleState.Table.GetLength(0);
        int cols = puzzleState.Table.GetLength(1);
        var grid = puzzleState.Table;
        var configs = puzzleState.TileConfigs;

        int[,] refillGrid = new int[rows, cols];

        for(int j=0; j<cols; ++j)
        {
            for(int i=0; i<rows; ++i) {
                refillGrid[i, j] = TileStateValue.Empty.SOEnumTypeInstanceId;
                if(grid[i,j].GameObjectInstanceId == TileStateValue.Empty.GameObjectInstanceId)
                {
                    var cnf = configs[UnityEngine.Random.Range(0, configs.Length-1)];
                    var tuple = InstantiateTile(
                        puzzleState.Prefab,
                        cnf,
                        puzzleState.transform,
                        new Vector2Int(i, j),
                        puzzleState.InputHandler);

                    puzzleState.Table[i, j] = tuple.Item1;
                    puzzleState.TilesRefComponents.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);

                    refillGrid[i, j] = tuple.Item1.GameObjectInstanceId;
                }
            }
        }

        return refillGrid;
    }
}
