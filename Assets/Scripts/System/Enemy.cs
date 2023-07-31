using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    private bool found = false;
    private bool trapped = false;
    private Tile hidingTile;
    private Tile currentTile = null;
    private float seekRange = 1;
    private float tetherRange = 1;
    private Vector2 startPos;

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
        startPos = pos;
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
            if (Vector2.Distance(targetPos, pos) > seekRange)
            {
                targetPos = MovePosition;
                //If already here,
                if (targetPos == pos)
                {
                    //if at start,
                    if (pos == startPos)
                    {
                    //change direction randomly
                    targetPos = new Vector2(
                        Random.Range(pos.x - tetherRange, pos.x + tetherRange),
                        Random.Range(pos.y - tetherRange, pos.y + tetherRange)
                        ).toVector2Int();
                    }
                    //go to start
                    else
                    {
                        targetPos = startPos;
                    }
                }
            }
            if (targetPos != MovePosition)
            {
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
            }
            move();
    }
}
