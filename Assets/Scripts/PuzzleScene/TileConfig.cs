using System;
using System.Collections.Generic;
using UnityEngine;

public enum TileBaseType
{
    Simple, Bomb, 
}

[Serializable]
public struct GenerationRequirement
{
    public int NumberOfRequiredItem;
    public TileConfig GeneratedType;
}

[CreateAssetMenu(menuName ="ScriptableObject/Config/EntityConfig")]
public class TileConfig : ScriptableObject
{
    [SerializeField] TileBaseType _baseType;            public TileBaseType BaseType => _baseType;
    [SerializeField] Sprite _sprite;                    public Sprite Sprite => _sprite;
    [SerializeField] GenerationRequirement[] _genReqs;  public IReadOnlyList<GenerationRequirement> GenerationReqs => _genReqs;
}