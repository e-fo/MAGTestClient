using System.Collections.Generic;

public static partial class ArrayUtil
{
    public static int CountNotEqual2D<T>(in T[,] array, T value)
    {
        int ret = 0;
        
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if(!EqualityComparer<T>.Default.Equals(array[i, j], value)) ret++;
            }
        }

        return ret;
    }
}