using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    /// <summary>
    /// How fast the player can move in units per second
    /// </summary>
    public float moveSpeed = 2;
    /// <summary>
    /// How far away the player can see from themselves
    /// </summary>
    public int visionRange = 5;
    /// <summary>
    /// How close the player must be to reveal a tile
    /// </summary>
    public int inspectRange = 2;

    public Game game;


    public Vector2 movePos = Vector2.zero;

    private Vector2 position;
    public Vector2 Position
    {
        get => position;
        set
        {
            position = value;
            gridPos = position.toVector2Int();
            onPositionChanged?.Invoke(position);
        }
    }
    public delegate void OnPositionChanged(Vector2 position);
    public event OnPositionChanged onPositionChanged;

    private Vector2Int gridPos;

    public void move()
    {
        if (Position != movePos)
        {
            Vector2 dir = (movePos - Position).normalized;
            Position += dir * Mathf.Min(
                moveSpeed * Time.deltaTime,
                Vector2.Distance(Position, movePos)
                );
            if (Mathf.Approximately(Vector2.Distance(Position, movePos), 0))
            {
                Position = movePos;
                onPosReached?.Invoke(Position);
            }
        }
    }
    public delegate void OnPosReached(Vector2 pos);
    public event OnPosReached onPosReached;

    public void tryReveal()
    {
        Tile moveTile = game.planet.map[movePos.toVector2Int()];
        if (!moveTile)
        {
            return;
        }
        if (moveTile.CanReveal && !moveTile.Revealed)
        {
            if (Vector2.Distance(movePos, Position) <= inspectRange)
            {
                moveTile.Revealed = true;
            }
        }
    }

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
