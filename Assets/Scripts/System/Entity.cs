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

    public delegate void OnPositionChanged(Vector2 position);

    private Vector2 movePos = Vector2.zero;
    public Vector2 MovePosition
    {
        get => movePos;
        set
        {
            movePos = value;
            onMovePositionChanged?.Invoke(movePos);
            if (movePos == position)
            {
                onPosReached?.Invoke(position);
            }
        }
    }
    public event OnPositionChanged onMovePositionChanged;

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
    public event OnPositionChanged onPositionChanged;
    private Vector2Int gridPos;

    public Entity(Game game)
    {
        this.game = game;
    }

    public virtual void process()
    {
        move();
    }

    public void move()
    {
        if (position != movePos)
        {
            Vector2 dir = (movePos - position).normalized;
            Position += dir * Mathf.Min(
                moveSpeed * Time.deltaTime,
                Vector2.Distance(position, movePos)
                );
            if (Mathf.Approximately(Vector2.Distance(position, movePos), 0))
            {
                Position = movePos;
                onPosReached?.Invoke(position);
            }
        }
    }
    public delegate void OnPosReached(Vector2 pos);
    public event OnPosReached onPosReached;

    public void stop()
    {
        //make it stop in the center of a tile, but don't make it go backwards
        MovePosition = ((movePos - position).resize(0.49f) + position).toVector2Int();
    }
}
