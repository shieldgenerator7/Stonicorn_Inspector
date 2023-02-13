using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private Tile tile;

    public void init(Tile tile)
    {
        this.tile = tile;
    }

    private Collider2D coll2d;

    private void Start()
    {
        coll2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bool leftclick = Input.GetMouseButtonDown(0);
        bool rightclick = Input.GetMouseButtonDown(1);
        bool overlap = false;
        if (leftclick || rightclick)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            overlap = coll2d.OverlapPoint(pos);
        }
        if (overlap)
        {
            if (leftclick)
            {
                FindObjectOfType<PlayerController>().movePos = transform.position;
                FindObjectOfType<PlayerController>().onPosReached -= revealThis;
                FindObjectOfType<PlayerController>().onPosReached += revealThis;
            }
            if (rightclick)
            {
                if (!tile.Revealed)
                {
                    tile.Flagged = !tile.Flagged;
                }
            }
        }
    }

    public void revealThis(Vector2 pos)
    {
        if (pos == (Vector2)transform.position)
        {
            if (!tile.Flagged)
            {
                tile.Revealed = true;
            }
        }
        FindObjectOfType<PlayerController>().onPosReached -= revealThis;
    }
}
