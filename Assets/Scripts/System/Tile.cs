using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector2Int position;
    public List<TileObject> objects;

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

    public static implicit operator bool(Tile tile) => tile != null;
}