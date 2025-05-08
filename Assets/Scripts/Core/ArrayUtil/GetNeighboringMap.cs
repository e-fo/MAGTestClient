using UnityEngine;

public static partial class ArrayUtil
{
    public static T[,] GetNeighboringMap<T>(
        in T[,] array, 
        Vector2Int idx, 
        int level, 
        T defaultValue, 
        bool justLastLevel = false)
    {
        T[,] ret = new T[array.GetLength(0), array.GetLength(1)];
        
        GetNeighboringMap(ref ret, array, idx, level, defaultValue, justLastLevel);

        return ret;
    }

    public static void GetNeighboringMap<T>(
        ref T[,] neighborMap,
        in T[,] array,
        Vector2Int idx, 
        int level, 
        T defaultValue, 
        bool justLastLevel = false)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        Fill2D(neighborMap, defaultValue);

        for (int i = idx.x - level; i <= idx.x + level; i++)
        {
            for (int j = idx.y - level; j <= idx.y + level; j++)
            {
                //check if the indices are within bounds and not equal to the center point
                if (i >= 0 && i < rows && j >= 0 && j < cols && (i != idx.x || j != idx.y))
                {
                    if(justLastLevel)
                    {
                        bool isLastLevel = (Mathf.Abs(i - idx.x) == level || Mathf.Abs(j - idx.y) == level);
                        if(isLastLevel) neighborMap[i, j] = array[i,j];
                    }
                    else neighborMap[i, j] = array[i,j];
                }
            }
        }
    }
}