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

    public Planet()
    {
        map = new Grid<Tile>();
        map.onGridChanged += (grid) => onMapChanged?.Invoke(grid);
    }

    public void generate(MapGenerator mapGenerator)
    {
        mapGenerator.generate(map);
        onMapChanged?.Invoke(map);
    }

    public delegate void OnMapChanged(Grid<Tile> map);
    public event OnMapChanged onMapChanged;
}
