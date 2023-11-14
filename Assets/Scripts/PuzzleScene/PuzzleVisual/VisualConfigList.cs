using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/VisualConfigList", order = 1)]
public class VisualConfigList : ScriptableObject
{
    [SerializeField] VisualConfigBase[] _list; public IReadOnlyList<VisualConfigBase> List => _list;

    public T Get<T>() where T : VisualConfigBase
    {
        string id = typeof(T).Name;
        return (T)_list.First(c=>c.Id==id);
    }
}
