using UnityEngine;

public static partial class PuzzleLogic
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="idGrid"></param>
    /// <param name="configs">list of instanceId of scriptable object configs</param>
    /// <returns>returns same size 2d map as grid. each element contains config instanceId (type) of new generataed tile. other elementes are Empty</returns>
    public static int[,] GenerateRefillMap(in int[,] idGrid, in int[] configs)
    {
        int rows = idGrid.GetLength(0);
        int cols = idGrid.GetLength(1);

        int[,] ret = new int[rows, cols];
        ArrayUtil.Fill2D(ret, TileStateValue.Empty.SOEnumTypeInstanceId);

        for(int j=0; j<cols; ++j)
            for(int i=0; i<rows; ++i) 
            {
                if(idGrid[i,j] == TileStateValue.Empty.GameObjectInstanceId)
                {
                    ret[i,j] = configs[Random.Range(0, configs.Length-1)];
                }
            }

        return ret;
    }
}
