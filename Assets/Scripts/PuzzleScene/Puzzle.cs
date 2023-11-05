using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public int Width = 7;
    public int Height = 7;
    public TileConfig[] TileConfigs;
    public GameObject Prefab;

    [NonSerialized] public TileStateValue[,] Table;
    /// <summary>
    /// stores all reference type states of tiles in a map
    /// (key: GameObject InstanceId, Val: Reference type components)
    /// </summary>
    [NonSerialized] public Dictionary<int, TileStateRef> TilesRefComponents = new();

    void Start()
    {
        //initialize table
        {
            Table = new TileStateValue[Width, Height];
            float startX = transform.position.x;
            float startY = transform.position.y;
            var inputHandler = new UnityAction<Vector2Int>(GetComponent<PuzzleInputHandler>().OnTapHandler);
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    var tuple = PuzzleUtil.InstantiateTile(
                        Prefab,
                        TileConfigs[UnityEngine.Random.Range(0, TileConfigs.Length)],
                        transform,
                        new Vector2(startX + x, startY + y),
                        inputHandler
                        );

                    Table[x, y] = tuple.Item1;
                    TilesRefComponents.Add(tuple.Item1.GameObjectInstanceId, tuple.Item2);
                }
            }
        }
    }
}