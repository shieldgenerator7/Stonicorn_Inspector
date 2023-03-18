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
    }
}
