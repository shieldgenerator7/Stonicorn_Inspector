using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<Detector> detectors = new List<Detector>();


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

    /// <summary>
    /// Reveals the destination tile if it can
    /// </summary>
    /// <returns>
    /// true if the tile is revealed afterwards.
    /// false if the tile is not revealed afterwards or is out of range.
    /// </returns>
    public bool tryReveal()
    {
        if (Vector2.Distance(movePos, Position) <= inspectRange)
        {
            Tile moveTile = game.planet.map[movePos.toVector2Int()];
            if (!moveTile)
            {
                return true;
            }
            if (moveTile.CanReveal)
            {
                if (!moveTile.Revealed)
                {
                    moveTile.Revealed = true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void placeDetector(Vector2Int pos)
    {
        //Early exit: already a detector at that pos
        if (detectors.Any(d => d.Position == pos))
        {
            return;
        }
        //
        Detector detector = new Detector(game.planet.map, 1);
        detector.detect(pos);
        detectors.Add(detector);
        onDetectorAdded?.Invoke(detector);
    }
    public delegate void OnDetectorEvent(Detector detectors);
    public event OnDetectorEvent onDetectorAdded;

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
