using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detector : Entity
{
    private int range;
    public int Range => range;

    private Vector2Int pos;
    public Vector2Int Pos => pos;

    private int detectedAmount;
    public int Detected
    {
        get => detectedAmount;
        private set
        {
            detectedAmount = value;
            onDetectedAmountChanged?.Invoke(detectedAmount);
        }
    }
    public delegate void OnDetectedAmountChanged(int amount);
    public event OnDetectedAmountChanged onDetectedAmountChanged;

    private Grid<Tile> map;

    public Detector(Game game, int range = 1) : base(game)
    {
        this.range = range;
    }

    public void detect()
    {
        detect(pos);
    }

    public void detect(Vector2Int pos)
    {
        this.pos = pos;
        Grid<Tile> map = game.planet.map;
        Detected = game.enemies.Count(
            enemy => Utility.WithinRangeInt(pos, enemy.Position, range)
                && !(map[enemy.Position.toVector2Int()]?.Flagged ?? false)
            );
    }

    public void destroy()
    {
        onDestroyed?.Invoke();
    }
    public delegate void OnDestroyed();
    public event OnDestroyed onDestroyed;
}
