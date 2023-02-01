using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplayer : MonoBehaviour
{
    public SpriteRenderer cover;

    private Tile tile;

    public void init(Tile tile)
    {
        this.tile = tile;
        registerDelegates(true);
        forceUpdate();
    }

    private void OnEnable()
    {
        if (!tile) { return; }
        registerDelegates(true);
        forceUpdate();
    }

    private void OnDisable()
    {
        if (!tile) { return; }
        registerDelegates(false);
    }

    private void registerDelegates(bool register)
    {
        tile.onRevealedChanged -= onRevealedChanged;
        if (register)
        {
            tile.onRevealedChanged += onRevealedChanged;
        }
    }

    private void forceUpdate()
    {
        onRevealedChanged(tile.Revealed);
    }

    private void onRevealedChanged(bool revealed)
    {
        cover.gameObject.SetActive(!revealed);
    }
}
