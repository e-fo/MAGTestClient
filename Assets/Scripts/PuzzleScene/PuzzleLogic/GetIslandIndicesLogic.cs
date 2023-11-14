using System.Collections.Generic;
using UnityEngine;

public static partial class PuzzleLogic
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="tapPos"></param>
    /// <returns>returns same size 2d map as grid. each element contains instanceId of island tile and the other elements are Empty</returns>
    public static int[,] GetIslandMap(in TileStateValue[,] grid, in Vector2Int tapPos)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        var typeGrid = GetTypeGrid(grid);
        var idGrid = GetIdGrid(grid);

        int[,] ret = new int[rows, cols];
        ArrayUtil.Fill2D(ret, TileStateValue.Empty.GameObjectInstanceId);

        bool[,] visited = new bool[rows, cols];
        int selected = typeGrid[tapPos.x, tapPos.y];

        Stack<Vector2Int> stack = new();
        stack.Push(tapPos);

        while(stack.Count > 0) {
            Vector2Int current = stack.Pop();
            int i = current.x;
            int j = current.y;

            if(i < 0 || j < 0 || i >= rows || j >= cols || visited[i, j] || typeGrid[i, j] != selected) {
                continue;
            }

            visited[i, j] = true;
            ret[i,j] = idGrid[i,j];

            stack.Push(new Vector2Int(i + 1, j)); 
            stack.Push(new Vector2Int(i - 1, j)); 
            stack.Push(new Vector2Int(i, j + 1));
            stack.Push(new Vector2Int(i, j - 1)); 
        }

        return ret;
    }

    public static Vector2Int[] GetIslandIndices(in int[,] typeGrid, in Vector2Int tapPos)
    {
        List<Vector2Int> ret = new();

        int rows = typeGrid.GetLength(0);
        int cols = typeGrid.GetLength(1);

        bool[,] visited = new bool[rows, cols];
        int selected = typeGrid[tapPos.x, tapPos.y];

        Stack<Vector2Int> stack = new();
        stack.Push(tapPos);

        while(stack.Count > 0) {
            Vector2Int current = stack.Pop();
            int i = current.x;
            int j = current.y;

            if(i < 0 || j < 0 || i >= rows || j >= cols || visited[i, j] || typeGrid[i, j] != selected) {
                continue;
            }

            visited[i, j] = true;
            ret.Add(new Vector2Int(i,j));
            stack.Push(new Vector2Int(i + 1, j)); 
            stack.Push(new Vector2Int(i - 1, j)); 
            stack.Push(new Vector2Int(i, j + 1));
            stack.Push(new Vector2Int(i, j - 1)); 
        }

        return ret.ToArray();
    }
}