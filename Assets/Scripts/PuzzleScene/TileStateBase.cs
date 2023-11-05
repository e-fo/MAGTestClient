using UnityEngine;

//we divided reference component types from value component types
//to decrease cache missed during iterating on puzzle table

/// <summary>
/// stores all values type components of a Tile
/// </summary>
public struct TileStateValue
{
    public readonly int GameObjectInstanceId;
    public readonly int SOEnumTypeInstanceId;

}

/// <summary>
/// stores all reference type components of a Tile
/// </summary>
public class TileStateRef
{
    public readonly Transform Transform;
    public readonly SOEnumTileType EntityType;
}