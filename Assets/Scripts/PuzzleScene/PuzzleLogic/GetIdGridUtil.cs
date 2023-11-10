public static partial class PuzzleLogic
{
    public static int[,] GetIdGrid(in TileStateValue[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        int[,] ret = new int[rows, cols];
        
        for(int i=0; i<rows; ++i) {
            for(int j=0; j<cols; ++j) {
                ret[i,j] = grid[i,j].GameObjectInstanceId;
            }
        }
        return ret;
    }
}