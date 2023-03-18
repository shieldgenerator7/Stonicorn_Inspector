using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static T randomItem<T>(this List<T> list)
    {
        int index = Random.Range(0,list.Count);
        return list[index];
    }
}
