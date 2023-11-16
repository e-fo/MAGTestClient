using System.Collections.Generic;
using UnityEngine;

public static partial class ArrayUtil
{
    public static T[,] GetIslandMap<T>(in T[,] array, Vector2Int index, T defaultValue)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        T[,] ret = new T[rows, cols];
        Fill2D(ret, defaultValue);
        bool[,] visited = new bool[rows, cols];

        T selected = array[index.x, index.y];

        Stack<Vector2Int> stack = new();
        stack.Push(index);

        while(stack.Count > 0) {
            Vector2Int current = stack.Pop();
            int i = current.x;
            int j = current.y;

            if(i < 0 || j < 0 || i >= rows || j >= cols || visited[i, j] || !EqualityComparer<T>.Default.Equals(array[i, j], selected)) {
                continue;
            }

            visited[i, j] = true;
            ret[i,j] = array[i,j];

            stack.Push(new Vector2Int(i + 1, j)); 
            stack.Push(new Vector2Int(i - 1, j)); 
            stack.Push(new Vector2Int(i, j + 1));
            stack.Push(new Vector2Int(i, j - 1)); 
        }

        return ret;
    }
}