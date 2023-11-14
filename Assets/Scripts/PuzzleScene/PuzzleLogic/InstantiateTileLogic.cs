using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static partial class PuzzleLogic
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
            new TileStateRef(tile.transform, tile.GetComponent<SpriteRenderer>()));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="puzzleState"></param>
    /// <param name="instantiateMap">same size 2d map as grid, each elements should contain type (config instanceId) of tile to be generated. other elements are Empty</param>
    /// <returns>return same 2d map as instantiateMap, but each element contains instanceId of new generated tile.</returns>
    public static int[,] InstantiateTileBatch(Puzzle puzzleState, int[,] instantiateMap)
    {
        int rows = instantiateMap.GetLength(0);
        int cols = instantiateMap.GetLength(1);

        int[,] ret = new int[rows, cols];
        ArrayUtil.Fill2D(ret, TileStateValue.Empty.GameObjectInstanceId);

        var configs = puzzleState.TileConfigs;
        var parent = puzzleState.transform;
        var prefab = puzzleState.Prefab;
        var input = puzzleState.InputHandler;
        var grid = puzzleState.Grid;
        var refs = puzzleState.TilesRefComponents;

        for(int i=0; i<rows; ++i)
            for(int j=0; j<cols; ++j)
            {
                int cnfId = instantiateMap[i,j];
                if(cnfId != TileStateValue.Empty.SOEnumTypeInstanceId)
                {
                    var cnf = configs.First(c=>c.GetInstanceID() == cnfId);
                    var tuple = InstantiateTile(prefab, cnf, parent, new Vector2Int(i, j), input);
                    grid[i, j] = tuple.Item1;
                    refs.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);

                    ret[i,j] = tuple.Item1.GameObjectInstanceId;
                }
            }

        return ret;
    }
}