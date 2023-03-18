using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public float moveSpeed = 2;
    public int visionRange = 5;
    public int inspectRange = 2;

    public Game game;


    public Vector2 movePos;

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
