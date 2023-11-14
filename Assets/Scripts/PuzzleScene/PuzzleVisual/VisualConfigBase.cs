using UnityEngine;

public class VisualConfigBase : ScriptableObject
{
    public string Id {get; private set;}
    protected virtual void OnEnable()
    {
        Id = this.GetType().Name;
    }
}
