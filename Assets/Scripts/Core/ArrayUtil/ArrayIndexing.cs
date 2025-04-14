using UnityEngine;

public static partial class ArrayUtil
{
    public static int CircularIndex(int i, int size) => Mathf.RoundToInt(CircularPosition(i, size));
    public static float CircularPosition(float p, int size) => size < 1 ? 0 : p < 0 ? size - 1 + (p + 1) % size : p % size;
}