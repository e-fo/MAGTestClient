public static partial class PuzzleLogic
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idGrid"></param>
    /// <returns>returns same size 2d map as grid. each element contains drop count of tile</returns>
    public static int[,] CalculateTilesDrop(in int[,] idGrid)
    {
        int rows = idGrid.GetLength(0);
        int cols = idGrid.GetLength(1);

        int[,] ret = new int[rows, cols];

        for (int i = 0; i < rows; ++i)
        {
            int dropCounter = 0;
            bool isPreviousEmpty = false;
            for (int j = 0; j <cols; ++j)
            {
                if(idGrid[i,j] == TileStateValue.Empty.GameObjectInstanceId)
                {
                    if(!isPreviousEmpty) dropCounter = 0;
                    dropCounter++;
                    isPreviousEmpty = true;
                }
                else
                {
                    ret[i,j] = dropCounter;
                    isPreviousEmpty = false;
                }
            }
        }

        return ret;
    }

    public static void ApplyTilesDrop(ref TileStateValue[,] grid, in int[,] dropMap) 
    {
        int rows = dropMap.GetLength(0);
        int cols = dropMap.GetLength(1);

        for (int i = 0; i < rows; ++i)
            for (int j = 0; j < cols; ++j)
            {
                int drop = dropMap[i, j];

                if(drop > 0)
                {
                    grid[i, (j - drop)] = grid[i,j];
                    grid[i,j] = TileStateValue.Empty;
                }
            }
    }
}