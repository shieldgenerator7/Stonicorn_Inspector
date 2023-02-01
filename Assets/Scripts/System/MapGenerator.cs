using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapGenerator : ScriptableObject
{
    public abstract void generate(Grid<Tile> grid);
}
