using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int visionRange = 5;
    public int inspectRange = 2;

    public Game game;

    public Vector2Int position;

    public void revealTile(Vector2Int position)
    {
        Grid<Tile> map = game.planet.map;
        Tile tile = map[position];
        if (tile)
        {
            tile.Revealed = true;
        }
    }

}
