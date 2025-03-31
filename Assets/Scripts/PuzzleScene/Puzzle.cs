using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public GameObject Prefab;
    public TileConfigList TileConfigs;
    public VisualConfigList VisualConfigs;
    [NonSerialized] public TileStateValue[,] Grid;

    private readonly Vector2Int INVALID_INPUT = new Vector2Int(-1, -1);
    private Vector2Int _selectedInput;

    /// <summary>
    /// stores all reference type states of tiles in a map
    /// (Key: GameObject InstanceId, Val: Reference type components)
    /// </summary>
    [NonSerialized] public Dictionary<int, TileStateRef> TilesRefComponents = new();

    public void InitPuzzle(LevelConfig level)
    {
        _selectedInput = INVALID_INPUT;
        int rows = level.RowsCount;
        int cols = level.ColsCount;
        //initialize grid
        {
            Grid = new TileStateValue[rows, cols];
            float startX = transform.position.x;
            float startY = transform.position.y;
            var confs = TileConfigs.List;
            for (int x = 0; x < rows; ++x)
            {
                for (int y = 0; y < cols; ++y)
                {
                    var tuple = InstantiateTile(
                        level[x, cols -1 - y], //sync initial level data grid view with Unity coordination
                        new Vector2(startX + x, startY + y)
                        );

                    Grid[x, y] = tuple.Item1;
                    TilesRefComponents.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);
                }
            }
        }

        //setup camera
        {
            var c = Camera.main;

            float orthoSize = (rows>cols)?rows:cols;
            float pos = orthoSize/2 - 0.5f;

            c.orthographicSize = orthoSize;
            c.transform.position = new Vector3(pos, pos, c.transform.position.z);
        }
    }

    public (TileStateValue, TileStateRef) InstantiateTile(in TileConfig conf, in Vector2 pos)
    {
        GameObject tile = GameObject.Instantiate(Prefab, pos, Quaternion.identity);
        tile.GetComponent<TileInputEvent>().OnTileTapped.AddListener(pos => _selectedInput = pos);
        tile.transform.parent = transform;
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
    public int[,] InstantiateTileBatch(Puzzle puzzleState, int[,] instantiateMap)
    {
        int rows = instantiateMap.GetLength(0);
        int cols = instantiateMap.GetLength(1);

        int[,] ret = new int[rows, cols];
        ArrayUtil.Fill2D(ret, TileStateValue.Empty.GameObjectInstanceId);


        for(int i=0; i<rows; ++i)
            for(int j=0; j<cols; ++j)
            {
                int cnfId = instantiateMap[i,j];
                if(cnfId != TileStateValue.Empty.SOEnumTypeInstanceId)
                {
                    var cnf = TileConfigs.List.First(c=>c.GetInstanceID() == cnfId);
                    var tuple = InstantiateTile(cnf, new Vector2Int(i, j));
                    Grid[i, j] = tuple.Item1;
                    TilesRefComponents.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);

                    ret[i,j] = tuple.Item1.GameObjectInstanceId;
                }
            }

        return ret;
    }

    public async UniTask<Vector2Int> GetPlayerInput()
    {
        await UniTask.WaitUntil(()=>_selectedInput!=INVALID_INPUT);

        Vector2Int ret = _selectedInput;
        _selectedInput = INVALID_INPUT;

        return ret;
    }
}