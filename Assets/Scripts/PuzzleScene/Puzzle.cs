using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public int Width = 7;
    public int Height = 7;
    public bool InputAvailable = true;
    public TileConfigList TileConfigs;
    public VisualConfigList VisualConfigs;
    public GameObject Prefab;
    public UnityAction<Vector2Int> InputHandler;
    [NonSerialized] public TileStateValue[,] Grid;

    /// <summary>
    /// stores all reference type states of tiles in a map
    /// (Key: GameObject InstanceId, Val: Reference type components)
    /// </summary>
    [NonSerialized] public Dictionary<int, TileStateRef> TilesRefComponents = new();

    void Start()
    {
        //initialize table
        {
            Grid = new TileStateValue[Width, Height];
            float startX = transform.position.x;
            float startY = transform.position.y;
            InputHandler = new UnityAction<Vector2Int>(GetComponent<PuzzleInputHandler>().OnTapHandler);
            var confs = TileConfigs.List;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var tuple = PuzzleLogic.InstantiateTile(
                        Prefab,
                        confs[UnityEngine.Random.Range(0, confs.Count)],
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