using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Entity
{
    private StonicornSettings settings;

    public enum Task
    {
        NONE,
        REVEAL,
        FLAG,
    }
    public Task task = Task.NONE;

    private Detector followDetector;
    public Detector FollowDetector => followDetector;
    private List<Detector> detectors = new List<Detector>();
    public int DetectorCount => detectors.Count;

    public Player(Game game, StonicornSettings settings) : base(game)
    {
        this.settings = settings;
        this.moveSpeed = settings.moveSpeed;

        onPositionChanged += (pos) => followDetector?.detect(pos.toVector2Int());
    }
    public void init(Planet planet)
    {
        followDetector = new Detector(game, 2);
        followDetector.detect(Position.toVector2Int());
    }

    public override void process()
    {
        move();
        switch (task)
        {
            case Task.REVEAL:
                tryReveal();
                break;
            case Task.FLAG:
                tryFlag();
                break;
            case Task.NONE: break;
            default: throw new UnityException($"Unknown task enum value: {task}");
        }
        autoPlaceDetectors();
    }

    /// <summary>
    /// Reveals the destination tile if it can
    /// </summary>
    /// <returns>
    /// true if the tile is revealed afterwards.
    /// false if the tile is not revealed afterwards or is out of range.
    /// </returns>
    public bool tryReveal()
    {
        if (WithinRangeInt(MovePosition))
        {
            Tile moveTile = game.planet.map[MovePosition.toVector2Int()];
            if (!moveTile)
            {
                return true;
            }
            if (moveTile.CanReveal)
            {
                if (!moveTile.Revealed)
                {
                    moveTile.Revealed = true;
                    OnTileRevealed?.Invoke(moveTile, true);
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
    public delegate void TileDelegate(Tile tile, bool state);
    public event TileDelegate OnTileRevealed;

    /// <summary>
    /// Flags or unflags the destination tile if it can
    /// </summary>
    /// <returns>
    /// true if it flagged or unflagged the tile
    /// false if the tile is null or out of range
    /// </returns>
    public bool tryFlag()
    {
        if (WithinRangeInt(MovePosition))
        {
            Tile moveTile = game.planet.map[MovePosition.toVector2Int()];
            if (!moveTile)
            {
                return false;
            }
            if (moveTile.CanFlag)
            {
                moveTile.Flagged = !moveTile.Flagged;
                OnTileFlagged?.Invoke(moveTile, moveTile.Flagged);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public event TileDelegate OnTileFlagged;

    public void autoPlaceDetectors()
    {
        //remove old detectors
        for (int i = detectors.Count - 1; i >= 0; i--)
        {
            removeDetector(i);
        }
        //add detectors on all squares in range
        Vector2Int posInt = Position.toVector2Int();
        for (int i = posInt.x - 2; i <= posInt.x + 2; i++)
        {
            for (int j = posInt.y - 2; j <= posInt.y + 2; j++)
            {
                Vector2Int pos = new Vector2Int(i, j);
                Tile tile = game.planet.map[pos];
                int hiddenNeighborCount = game.planet.map.getNeighborCount(
                    pos, 1, (t => t && !t.Revealed && !t.Flagged)
                    );
                if ((!tile || tile.Revealed) && hiddenNeighborCount > 0)
                {
                    placeDetector(pos);
                }
            }
        }
    }

    public Detector placeDetector(Vector2Int pos, int range = 1)
    {
        //Early exit: already a detector at that pos
        Detector existing = detectors.Find(d => d.Pos == pos);
        if (existing != null)
        {
            return existing;
        }
        //
        Detector detector = new Detector(game, range);
        detector.detect(pos);
        detectors.Add(detector);
        onDetectorAdded?.Invoke(detector);
        return detector;
    }
    public delegate void OnDetectorEvent(Detector detectors);
    public event OnDetectorEvent onDetectorAdded;

    public void removeDetector(int index = 0)
    {
        Detector detector = detectors[index];
        detector.destroy();
        detectors.RemoveAt(index);
        onDetectorRemoved?.Invoke(detector);
    }
    public void removeDetector(Vector2 pos)
    {
        Detector detector = detectors.Find(detector => detector.Pos == pos);
        detector.destroy();
        detectors.Remove(detector);
        onDetectorRemoved?.Invoke(detector);
    }
    public event OnDetectorEvent onDetectorRemoved;

    public void revealTile(Vector2Int position)
    {
        Grid<Tile> map = game.planet.map;
        Tile tile = map[position];
        if (tile)
        {
            tile.Revealed = true;
            OnTileRevealed?.Invoke(tile, true);
        }
    }

    public bool WithinRangeInt(Vector2 pos)
    {
        return WithinRangeInt(pos.toVector2Int());
    }
    public bool WithinRangeInt(Vector2Int pos)
    {
        return Utility.WithinRangeInt(pos, Position, settings.inspectRange);
    }

}
