using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMapGenerator<T> : MapGenerator<T> where T : class
{
    public int radius = 5;
    public override void generate(Grid<T> grid)
    {
        for(int x = -radius; x <= radius; x++)
        {
            for(int y = -radius; y <= radius; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (pos.magnitude <= radius)
                {
                    grid.add(pos, default);
                }
            }
        }
    }
}
