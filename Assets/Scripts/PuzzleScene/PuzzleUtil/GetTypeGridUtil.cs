public static partial class PuzzleUtil
{
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
}