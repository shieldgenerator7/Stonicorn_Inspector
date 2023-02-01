using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetDisplayer : MonoBehaviour
{
    public GameObject tilePrefab;

    private Planet planet;

    private List<TileDisplayer> tiles = new List<TileDisplayer>();

    public void init(Planet planet)
    {
        this.planet = planet;
        planet.onMapChanged += generateDisplay;
    }

    private void generateDisplay(Grid<Tile> map)
    {
        //Clear old tiles
        tiles.ForEach(tile => Destroy(tile.gameObject));
        tiles.Clear();
        //Create new tiles
        map.positions.ForEach(position =>
            tiles.Add(createTile(position, map[position]))
        );
        //Remove empties
        tiles.RemoveAll(tile => !tile);
    }

    private TileDisplayer createTile(Vector2Int v, Tile tile)
    {
        if (!tile)
        {
            Debug.LogError($"Tile {tile} is null at position {v}");
            return null;
        }
        GameObject go = Instantiate(tilePrefab, transform);
        go.transform.position = (Vector2)v;
        TileDisplayer tileDisplayer = go.GetComponent<TileDisplayer>();
        tileDisplayer.init(tile);
        TileController tileController = go.GetComponent<TileController>();
        tileController.init(tile);
        return tileDisplayer;
    }
}
