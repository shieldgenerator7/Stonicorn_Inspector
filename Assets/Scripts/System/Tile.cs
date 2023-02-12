using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2Int position;
    public List<TileObject> objects = new List<TileObject>();
    public int hazardCount = 0;

    private bool revealed = false;
    public bool Revealed
    {
        get => revealed;
        set
        {
            revealed = value;
            onRevealedChanged?.Invoke(revealed);
        }
    }
    public delegate void OnBool(bool value);
    public event OnBool onRevealedChanged;

    private bool flagged = false;
    public bool Flagged
    {
        get => flagged;
        set
        {
            flagged = value;
            onFlaggedChanged?.Invoke(flagged);
        }
    }
    public event OnBool onFlaggedChanged;

    public Tile(Vector2Int pos)
    {
        this.position = pos;
    }

    public static implicit operator bool(Tile tile) => tile != null;
    public override string ToString() => $"Tile ({position.x}, {position.y})";
}
