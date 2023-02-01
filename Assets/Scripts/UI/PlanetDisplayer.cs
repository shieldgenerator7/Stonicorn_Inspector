using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDisplayer : MonoBehaviour
{
    public GameObject tilePrefab;

    private Planet planet;

    private List<GameObject> tiles = new List<GameObject>();

    public void init(Planet planet)
    {
        this.planet = planet;
        planet.onMapChanged += generateDisplay;
    }

    private void generateDisplay(Grid<Tile> map)
    {
        //Clear old tiles
        tiles.ForEach(tile => Destroy(tile));
        tiles.Clear();
        //Create new tiles
        map.positions.ForEach(position =>
            tiles.Add(createTile(position))
        );
    }

    private GameObject createTile(Vector2Int v)
    {
        GameObject tile = Instantiate(tilePrefab, transform);
        tile.transform.position = (Vector2)v;
        return tile;
    }
}
