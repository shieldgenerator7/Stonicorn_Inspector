using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    #region Vector Extensions

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

    public static int DistanceInt(Vector2Int vi1, Vector2Int vi2)
    {
        //includes moving diagonally as one move
        return Mathf.Max(
            Mathf.Abs(vi1.x - vi2.x),
            Mathf.Abs(vi1.y - vi2.y)
            );
    }

    #endregion

    public static T randomItem<T>(this List<T> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }

}
