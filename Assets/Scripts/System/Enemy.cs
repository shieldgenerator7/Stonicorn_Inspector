using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private bool found = false;
    private bool flagged = false;
    private Tile hidingTile;
    private Tile currentTile = null;

    public Enemy(Game game) : base(game)
    {
        moveSpeed = game.player.moveSpeed / 4;
    }
    public void init(Vector2Int pos)
    {
        onPositionChanged += (pos) =>
        {
            Tile newTile = game.planet.map[pos.toVector2Int()];
            if (currentTile != newTile)
            {
                //detach from old tile
                if (currentTile)
                {
                    currentTile.onFlaggedChanged -= onFlaggedChanged;
                }
                //attach to new tile
                currentTile = newTile;
                if (currentTile)
                {
                    currentTile.onFlaggedChanged += onFlaggedChanged;
                }
            }
        };
        Position = pos;
        MovePosition = pos;
        hidingTile = game.planet.map[pos];
        found = hidingTile.Revealed;
        hidingTile.onRevealedChanged += OnFound;
    }

    void onFlaggedChanged(bool flagged)
    {
        this.flagged = flagged;
        if (flagged)
        {
            MovePosition = Position;
        }
    }

    void OnFound(bool found)
    {
        this.found = found;
        hidingTile.onRevealedChanged -= OnFound;
        hidingTile = null;
        onPosReached += (pos) =>
        {
            if (pos == game.player.Position)
            {
                game.player.moveSpeed = 0;
            }
        };
    }

    public override void process()
    {
        if (found)
        {
            if (!flagged)
            {
                MovePosition = game.player.Position;
            }
            move();
        }
    }
}
