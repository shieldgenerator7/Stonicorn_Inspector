using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private bool found = false;
    private Tile hidingTile;

    public Enemy(Game game) : base(game)
    {
        moveSpeed = game.player.moveSpeed / 4;
    }
    public void init(Vector2Int pos)
    {
        Position = pos;
        movePos = pos;
        hidingTile = game.planet.map[pos];
        found = hidingTile.Revealed;
        hidingTile.onRevealedChanged += OnFound;
    }

    void OnFound(bool found)
    {
        this.found = found;
        hidingTile.onRevealedChanged -= OnFound;
        hidingTile = null;
        onPosReached += (pos) => game.player.moveSpeed = 0;
    }

    public void process()
    {
        if (found)
        {
            movePos = game.player.Position;
            move();
        }
    }
}
