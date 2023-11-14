using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/Config/Tile/TileConfigList", order = 1)]
public class TileConfigList : ScriptableObject
{
    [SerializeField] TileConfig[] _list; public IReadOnlyList<TileConfig> List => _list;
}