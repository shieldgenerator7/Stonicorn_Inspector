using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        while (hazards < hazardCount)
        {
            Vector2Int pos = new Vector2Int(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
                );
            Tile tile = grid[pos];
            if (tile && tile.objects.Count == 0)
            {
                Hazard hazard = new Hazard();
                tile.objects.Add(hazard);
                hazards++;
            }
        }
    }
}
