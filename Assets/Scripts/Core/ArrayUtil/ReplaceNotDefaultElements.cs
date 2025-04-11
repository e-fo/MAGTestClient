using System.Collections.Generic;

public static partial class ArrayUtil
{
    public static void ReplaceNotDefaultElements<T>(ref T[,] array, in T[,] replaceValues, T defaultValue)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if(!EqualityComparer<T>.Default.Equals(array[i, j], defaultValue))
                    array[i, j] = replaceValues[i,j];
            }
        }
    }
}