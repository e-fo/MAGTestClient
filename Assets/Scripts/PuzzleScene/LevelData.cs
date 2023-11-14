using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/LevelData")]
public class LevelData : ScriptableObject
{
    [Serializable] public class TileList { public TileConfig[] Rows; }

    public TileList[] Columns;
    public int RowsCount => Columns.Length;
    public int ColsCount => Columns[0].Rows.Length;

    public TileConfig this[int i, int j]
    {
        get { return Columns[i].Rows[j]; }
        set { Columns[i].Rows[j] = value; }
    }

    public void SetSize(int rows, int cols)
    {
        TileList[] newColumns = new TileList[cols];
        for (int i = 0; i < cols; ++i)
        {
            if (i < RowsCount)
            {
                newColumns[i] = new TileList { Rows = new TileConfig[rows] };

                for (int j = 0; j < rows && j < Columns[i].Rows.Length; ++j)
                {
                    newColumns[i].Rows[j] = Columns[i].Rows[j];
                }
            } else
            {
                newColumns[i] = new TileList { Rows = new TileConfig[rows] };
            }
        }
        Columns = newColumns;
    }
}