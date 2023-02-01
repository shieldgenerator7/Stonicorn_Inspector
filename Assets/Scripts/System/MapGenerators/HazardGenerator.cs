using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hazard", menuName = "MapGenerator/Hazard")]
public class HazardGenerator : MapGenerator
{
    public int hazardCount = 12;
    public override void generate(Grid<Tile> grid)
    {
        Vector2Int min = grid.Min;
        Vector2Int max = grid.Max;
        int hazards = 0;
        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                if (hazards == hazardCount)
                {
                    return;
                }
                Vector2Int pos = new Vector2Int(x, y);
                Tile tile = grid[pos];
                if (tile)
                {                    
                    Hazard hazard = new Hazard();
                    tile.objects.Add(hazard);
                    hazards++;
                }
            }
        }
    }
}
