using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerator<T> : ScriptableObject where T : class
{
    public abstract void generate(Grid<T> grid);
}
