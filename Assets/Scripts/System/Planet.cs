using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    public string name;
    public Grid<Tile> map = new Grid<Tile>();
    public Vector2Int Size => map.Max - map.Min;
}
