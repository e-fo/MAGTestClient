using UnityEngine;

//we divided reference type states from value type states
//to decrease cache missed during iterating on puzzle table

/// <summary>
/// stores all values type states of a Tile
/// </summary>
public struct TileStateValue
{
    public static readonly TileStateValue Empty = new TileStateValue(-1,-1);

    public TileStateValue(int gameObjectInstanceId, int typeInstanceId)
    {
        GameObjectInstanceId= gameObjectInstanceId;
        SOEnumTypeInstanceId= typeInstanceId;
    }
    public readonly int GameObjectInstanceId;
    public readonly int SOEnumTypeInstanceId;
}

/// <summary>
/// stores all reference type states of a Tile
/// </summary>
public class TileStateRef
{
    public TileStateRef(Transform transform)
    {
        Transform = transform;
    }
    public readonly Transform Transform;
}