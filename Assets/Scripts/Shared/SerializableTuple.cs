using System;
using UnityEngine;

[Serializable]
public class SerializableTuple<T1,T2>
{
    public T1 Item1;
    public T2 Item2;
}

/// <summary>
/// Readonly version, but it can serialized on inspector.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
[Serializable]
public class ROSerializableTuple<T1,T2>
{
    [SerializeField] T1 _item1; public T1 Item1 => _item1;
    [SerializeField] T2 _item2; public T2 Item2 => _item2;
}

[Serializable]
public class SerializableTuple<T1,T2,T3>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
}

/// <summary>
/// Readonly version, but it can serialize on inspector.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
[Serializable]
public class ROSerializableTuple<T1,T2,T3>
{
    [SerializeField] T1 _item1; public T1 Item1 => _item1;
    [SerializeField] T2 _item2; public T2 Item2 => _item2;
    [SerializeField] T3 _item3; public T3 Item3 => _item3;
}

[Serializable]
public class SerializableTuple<T1,T2,T3,T4>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
}

/// <summary>
/// Readonly version, but it can serialize on inspector.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
[Serializable]
public class ROSerializableTuple<T1,T2,T3,T4>
{
    [SerializeField] T1 _item1; public T1 Item1 => _item1;
    [SerializeField] T2 _item2; public T2 Item2 => _item2;
    [SerializeField] T3 _item3; public T3 Item3 => _item3;
    [SerializeField] T4 _item4; public T4 Item4 => _item4;
}