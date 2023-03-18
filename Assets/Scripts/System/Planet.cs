using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Planet
{
    public string name;
    public Grid<Tile> map { get; private set; }
    public Vector2Int Size => map.Max - map.Min;

    public Planet(Grid<Tile> map)
    {
        this.map = map;
        this.map.onGridChanged += (grid) => onMapChanged?.Invoke(grid);
    }
    public delegate void OnMapChanged(Grid<Tile> map);
    public event OnMapChanged onMapChanged;
}
