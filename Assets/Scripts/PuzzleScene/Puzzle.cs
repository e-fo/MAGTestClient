using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public bool InputAvailable = true;
    public TileConfigList TileConfigs;
    public VisualConfigList VisualConfigs;
    public GameObject Prefab;
    public UnityAction<Vector2Int> InputHandler;
    [NonSerialized] public TileStateValue[,] Grid;
    [SerializeField] private LevelConfigList _levelList;
    [SerializeField] private IntVariable _currentLvlIdx;

    /// <summary>
    /// stores all reference type states of tiles in a map
    /// (Key: GameObject InstanceId, Val: Reference type components)
    /// </summary>
    [NonSerialized] public Dictionary<int, TileStateRef> TilesRefComponents = new();

    void Start()
    {
        //initialize table
        {
            var level = _levelList.List[_currentLvlIdx.Value];
            Grid = new TileStateValue[level.RowsCount, level.ColsCount];
            float startX = transform.position.x;
            float startY = transform.position.y;
            InputHandler = new UnityAction<Vector2Int>(GetComponent<PuzzleInputHandler>().OnTapHandler);
            var confs = TileConfigs.List;
            for (int x = 0; x < level.RowsCount; ++x)
            {
                for (int y = 0; y < level.ColsCount; ++y)
                {
                    var tuple = PuzzleLogic.InstantiateTile(
                        Prefab,
                        level[x, level.ColsCount -1 - y], //sync initial level data grid view with Unity coordination
                        transform,
                        new Vector2(startX + x, startY + y),
                        InputHandler
                        );

                    Grid[x, y] = tuple.Item1;
                    TilesRefComponents.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);
                }
            }
        }
    }
}