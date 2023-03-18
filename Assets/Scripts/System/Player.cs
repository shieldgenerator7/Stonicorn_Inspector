using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Entity
{
    /// <summary>
    /// How far away the player can see from themselves
    /// </summary>
    public int visionRange = 5;
    /// <summary>
    /// How close the player must be to reveal a tile
    /// </summary>
    public int inspectRange = 2;

    private Detector followDetector;
    public Detector FollowDetector => followDetector;
    private List<Detector> detectors = new List<Detector>();
    public int DetectorCount => detectors.Count;

    public Player(Game game) : base(game)
    {
        onPositionChanged += (pos) => followDetector?.detect(pos.toVector2Int());
    }
    public void init(Planet planet)
    {
        followDetector = new Detector(game, 2);
        followDetector.detect(Position.toVector2Int());
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

    public void removeDetector()
    {
        Detector detector = detectors[0];
        detector.destroy();
        detectors.RemoveAt(0);
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
        }
    }

}
