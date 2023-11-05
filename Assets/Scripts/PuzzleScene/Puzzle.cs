using System;
using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    public int Width = 7;
    public int Height = 7;
    public GameObject[] ElementsPrefab;

    [NonSerialized] public GameObject[,] Grid;

    void Start()
    {
        generateTable(Grid, this.transform, ElementsPrefab, new Vector2Int(Width, Height), OnTappedTile);
        static void generateTable(
            GameObject[,] grid,
            Transform parent,
            in GameObject[] prefabs,
            in Vector2Int tableSize,
            UnityAction<Vector2Int> onTappedInputHandler)
        {
            grid = new GameObject[tableSize.x, tableSize.y];
            float startX = parent.position.x;
            float startY = parent.position.y;

            for (int x = 0; x < tableSize.x; x++)
            {
                for (int y = 0; y < tableSize.y; y++)
                {
                    Vector2 spawnPosition = new Vector2(startX + x, startY + y);

                    GameObject tile = Instantiate(
                        prefabs[UnityEngine.Random.Range(0, prefabs.Length)], 
                        spawnPosition, 
                        Quaternion.identity);

                    tile.GetComponent<TileInputEvent>()
                        .OnTileTapped
                        .AddListener(onTappedInputHandler);

                    tile.transform.parent = parent;
                    grid[x, y] = tile;
                }
            }
        }
    }
    
    void OnTappedTile(Vector2Int position)
    {
        
    }
}