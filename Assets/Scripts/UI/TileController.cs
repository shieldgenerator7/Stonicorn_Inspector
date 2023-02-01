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
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log($"Click {tile}");
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (coll2d.OverlapPoint(pos))
            {
                Debug.Log($"Clicked on {tile}");
                tile.Revealed = true;
            }
        }
    }
}
