using UnityEngine;

public class SOVariableBase<T> : ScriptableObject
{
#if UNITY_EDITOR
    [TextArea]
    [SerializeField] string _developerDescription;
#endif

    public T Value;
    [SerializeField] T _initialValue;

    protected virtual void OnEnable() { Value = _initialValue; }
}