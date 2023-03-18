using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    public Game game;

    /// <summary>
    /// How fast the player can move in units per second
    /// </summary>
    public float moveSpeed = 2;

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

    public Entity(Game game)
    {
        this.game = game;
    }

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
}
