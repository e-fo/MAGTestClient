using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PuzzleUtil
{
    public static (TileStateValue, TileStateRef) InstantiateTile(
        in GameObject prefab, 
        in TileConfig conf, 
        in Transform parent, 
        in Vector2 pos, 
        in UnityAction<Vector2Int> onTapHandler)
    {
        GameObject tile = GameObject.Instantiate(prefab, pos, Quaternion.identity);
        tile.GetComponent<TileInputEvent>().OnTileTapped.AddListener(onTapHandler);
        tile.transform.parent = parent;
        tile.GetComponent<SpriteRenderer>().sprite = conf.Sprite;
        int configInstanceId = conf.GetInstanceID();
        return (
            new TileStateValue(tile.GetInstanceID(), configInstanceId), 
            new TileStateRef(tile.transform));
    }

    public static int[,] GetTypeGrid(in TileStateValue[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        int[,] ret = new int[rows, cols];
        
        for(int i=0; i<rows; ++i) {
            for(int j=0; j<cols; ++j) {
                ret[i,j] = grid[i,j].SOEnumTypeInstanceId;
            }
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