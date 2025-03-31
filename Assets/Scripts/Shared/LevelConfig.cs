using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelGoal
{
    public enum Type { TypeLess, BaseType, SpecificType }
    
    public static string GoalTypePropertyName => nameof(goalType); //customPropertyDrawer
    public Type GoalType => goalType;
    [SerializeField] Type goalType;

    public static string CountPropertyName => nameof(count);
    public int Amount => count;
    [SerializeField] int count;

    //specific type
    public static string TileConfigPropertyName => nameof(tileConfig);
    public TileConfig TileConfig => tileConfig;
    [SerializeField] TileConfig tileConfig;       

    //base type
    public static string TileBaseTypePropertyName => nameof(tileBaseType);
    public TileBaseType TileBaseType => tileBaseType;
    [SerializeField] TileBaseType tileBaseType;
}

[CreateAssetMenu(menuName = "ScriptableObject/Config/Level/LevelConfig", order = 2)]
public class LevelConfig : ScriptableObject
{
    [Serializable] public class TileList { public TileConfig[] Rows; }

    [SerializeField] int _totalMove;        public int TotalMove => _totalMove;
    [SerializeField] LevelGoal[] _goals;    public IReadOnlyList<LevelGoal> Goals=> _goals;

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