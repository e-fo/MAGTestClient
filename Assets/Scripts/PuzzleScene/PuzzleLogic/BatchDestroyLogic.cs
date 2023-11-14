using UnityEngine;

public static partial class PuzzleLogic
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="puzzleState"></param>
    /// <param name="destroyMap">same size 2d map as grid. each element should contain instanceId of tile to be destroyed. other elements are Empty</param>
    public static void DestroyTileBatch(Puzzle puzzleState, in int[,] destroyMap)
    {
        int rows = destroyMap.GetLength(0);
        int cols = destroyMap.GetLength(1);
        var refs = puzzleState.TilesRefComponents;
        var grid = puzzleState.Grid;

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
            {
                int instanceId = destroyMap[i, j];
                if (instanceId != TileStateValue.Empty.GameObjectInstanceId)
                {
                    puzzleState.Grid[i, j] = TileStateValue.Empty;
                    Object.Destroy(puzzleState.TilesRefComponents[instanceId].Transform.gameObject);
                    puzzleState.TilesRefComponents.Remove(instanceId);
                }
            }
    }
}