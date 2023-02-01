using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "MapGenerator/Planet")]
public class PlanetMapGenerator : MapGenerator
{
    public int radius = 5;
    public override void generate(Grid<Tile> grid)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                if (pos.magnitude <= radius)
                {
                    grid.add(pos, new Tile(pos));
                }
            }
        }
    }
}
