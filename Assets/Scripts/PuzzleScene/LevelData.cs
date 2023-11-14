using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/LevelData")]
public class LevelData : ScriptableObject
{
    [Serializable] public struct TileList { public TileConfig[] Rows; }

    public TileList[] Columns;
    public int Rows => Columns.Length;
    public int Cols => Columns[0].Rows.Length;

    public TileConfig this[int i, int j]
    {
        get { return Columns[i].Rows[j];}
        set { Columns[i].Rows[j] = value;}
    }
}