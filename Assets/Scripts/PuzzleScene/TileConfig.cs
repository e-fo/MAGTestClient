using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/Config/EntityConfig")]
public class TileConfig : ScriptableObject
{
    [SerializeField] Sprite _sprite; public Sprite Sprite => _sprite;
}