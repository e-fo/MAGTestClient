using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/Level/LevelDataList", order = 1)]
public class LevelConfigList : SOReadonlyListBase<LevelConfig> 
{
    [Header("General Level Configs --------")]
    [SerializeField] Sprite _typeLessGoalSprite; public Sprite TypeLessGoalSprite => _typeLessGoalSprite;

    [SerializeField] ROSerializableTuple<TileBaseType, Sprite>[] _lvlGoalBaseTypeSpriteMap;
    public IReadOnlyList<ROSerializableTuple<TileBaseType, Sprite>> LvlGoalBaseTypeSpriteMap => _lvlGoalBaseTypeSpriteMap;
}