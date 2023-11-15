using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Config/PuzzleVisual/VisualConfigList", order = 1)]
public class VisualConfigList : SOReadonlyListBase<VisualConfigBase>
{
    public T Get<T>() where T : VisualConfigBase
    {
        string id = typeof(T).Name;
        return (T)_list.First(c=>c.Id==id);
    }
}
