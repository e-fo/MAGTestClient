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
    [SerializeField] private LevelData level;

    /// <summary>
    /// stores all reference type states of tiles in a map
    /// (Key: GameObject InstanceId, Val: Reference type components)
    /// </summary>
    [NonSerialized] public Dictionary<int, TileStateRef> TilesRefComponents = new();

    void Start()
    {
        //initialize table
        {
            Grid = new TileStateValue[level.Rows, level.Cols];
            float startX = transform.position.x;
            float startY = transform.position.y;
            InputHandler = new UnityAction<Vector2Int>(GetComponent<PuzzleInputHandler>().OnTapHandler);
            var confs = TileConfigs.List;
            for (int x = 0; x < level.Rows; ++x)
            {
                for (int y = 0; y < level.Cols; ++y)
                {
                    var tuple = PuzzleLogic.InstantiateTile(
                        Prefab,
                        //confs[UnityEngine.Random.Range(0, confs.Count)],
                        level[x,y],
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