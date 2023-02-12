using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDisplayer : MonoBehaviour
{
    public SpriteRenderer cover;
    public SpriteRenderer contents;
    public SpriteRenderer detector;
    public SpriteRenderer detectorFill;

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
        //Contents
        int contentCount = tile.objects.Count;
        contents.gameObject.SetActive(contentCount > 0);
        //Detector
        detector.gameObject.SetActive(
            tile.hazardCount > 0 && contentCount == 0
            );
        detector.sprite = spritesDetector[tile.hazardCount];
        detectorFill.sprite = spritesDetectorFill[tile.hazardCount];
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
