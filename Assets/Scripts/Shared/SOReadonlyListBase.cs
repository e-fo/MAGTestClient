using System.Collections.Generic;
using UnityEngine;

public class SOReadonlyListBase<T> : ScriptableObject
{
    [SerializeField] protected T[] _list; public IReadOnlyList<T> List => _list;
}
