using UnityEngine;

public class BombExplosionRule : IRuleTileTap
{
    public TileBaseType[] AcceptedBaseTypes => new TileBaseType[]{ TileBaseType.Bomb };

    public void Execute(Vector2Int position, Puzzle puzzle)
    {
        
    }
}