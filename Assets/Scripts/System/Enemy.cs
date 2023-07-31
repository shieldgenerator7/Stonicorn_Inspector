using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private bool found = false;
    private bool trapped = false;
    private Tile hidingTile;
    private Tile currentTile = null;
    private float seekRange = 2;
    private float randomDirectionChangeChance = 0.001f;

    public Enemy(Game game) : base(game)
    {
        moveSpeed = game.player.moveSpeed / 2;
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
        this.trapped = flagged;
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
            //Target player
            Vector2 targetPos = game.player.Position;
            //Target random direction
            Vector2 pos = Position;
            if (!found || Vector2.Distance(targetPos, pos) > seekRange)
            {
                //Random chance of changing direction
                if (Random.Range(0, 1) <= randomDirectionChangeChance)
                {
                    targetPos = new Vector2(
                        Random.Range(pos.x - 3, pos.x + 3),
                        Random.Range(pos.y - 3, pos.y + 3)
                        );
                }
            }
            //Move towards player
            if (!trapped)
            {
                MovePosition = targetPos;
            }
            //Move as close to player as possible while trapped
            else
            {
                MovePosition = (targetPos - pos).normalized * 0.3f + currentTile.position;
            }
            move();
    }
}
