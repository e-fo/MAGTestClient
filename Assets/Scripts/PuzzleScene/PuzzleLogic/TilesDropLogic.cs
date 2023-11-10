public static partial class PuzzleLogic
{
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
                if(idGrid[i,j] == -1)
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

    public static void ApplyTilesDrop(Puzzle puzzleState, in int[,] dropMap) 
    {
        int rows = dropMap.GetLength(0);
        int cols = dropMap.GetLength(1);
        var grid = puzzleState.Table;

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