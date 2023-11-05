using System.Collections.Generic;
using UnityEngine;

public interface IRuleTileTap
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="position">stores tapped position</param>
    /// <param name="table">stores table data</param>
    /// <param name="tileRefComponentMap"></param>
    public void Execute(Vector2 position, ref TileStateValue[,] table, ref Dictionary<int, TileStateRef> tileRefComponentMap);
}