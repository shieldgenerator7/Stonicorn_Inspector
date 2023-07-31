using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    #region Vector Extensions

    public const float VECTOR_APPROX_THRESHOLD = 0.1f;

    public static Vector2Int toVector2Int(this Vector2 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }
    public static Vector2Int toVector2Int(this Vector3 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static Vector2 resize(this Vector2 v, float distance)
    {
        return v.normalized * distance;
    }

    public static bool Approximately(this Vector2 v1, Vector2 v2)
    {
        return Vector2.Distance(v1, v2) <= VECTOR_APPROX_THRESHOLD;
    }

    public static bool Approximately(this Vector3 v1, Vector3 v2)
    {
        return Vector3.Distance(v1, v2) <= VECTOR_APPROX_THRESHOLD;
    }

    public static int DistanceInt(Vector2Int vi1, Vector2Int vi2)
    {
        //includes moving diagonally as one move
        return Mathf.Max(
            Mathf.Abs(vi1.x - vi2.x),
            Mathf.Abs(vi1.y - vi2.y)
            );
    }
    public static bool WithinRangeInt(Vector2 v1, Vector2 v2, int range)
    {
        return WithinRangeInt(v1.toVector2Int(), v2.toVector2Int(), range);
    }
    public static bool WithinRangeInt(Vector2Int vi1, Vector2Int vi2, int range)
    {
        return DistanceInt(vi1, vi2) <= range;
    }

    #endregion

    public static T randomItem<T>(this List<T> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }

}
