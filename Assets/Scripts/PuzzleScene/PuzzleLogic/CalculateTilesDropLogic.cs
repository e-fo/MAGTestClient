public static partial class PuzzleLogic
{
    public static int[,] CalculateTilesDrop(in int[,] idGrid)
    {
        int rows = idGrid.GetLength(0);
        int cols = idGrid.GetLength(1);

        int[,] ret = new int[rows, cols];

        for (int i = 0; i < rows; ++i)
            for (int j = 0; j < cols; ++j)
            {
                if (idGrid[i, j] != -1)
                {
                    for (int x = 0; x < rows - j; ++x)
                    {
                        int id = idGrid[i, j - x];
                        if (-1 == id) ret[i,j]++;
                        else break;
                    }
                }
            }

        return ret;
    }
}