using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileDisplayer : MonoBehaviour
{
    public SpriteRenderer cover;

    public SpriteSet spritesDetector;
    public SpriteSet spritesDetectorFill;

    public GameObject flag;

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
        tile.onFlaggedChanged -= onFlaggedChanged;
        if (register)
        {
            tile.onRevealedChanged += onRevealedChanged;
            tile.onFlaggedChanged += onFlaggedChanged;
        }
    }

    private void forceUpdate()
    {
        onRevealedChanged(tile.Revealed);
        onFlaggedChanged(tile.Flagged);
    }

    private void onRevealedChanged(bool revealed)
    {
        cover.gameObject.SetActive(!revealed);
    }

    private void onFlaggedChanged(bool flagged)
    {
        flag.SetActive(flagged);
    }
}
