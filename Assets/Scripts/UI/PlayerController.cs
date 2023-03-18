using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Receive input
        bool leftclick = Input.GetMouseButtonDown(0);
        bool rightclick = Input.GetMouseButtonDown(1);
        Vector2 mousePos = Vector2.zero;
        Tile overlapTile = null;
        if (leftclick || rightclick)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            overlapTile = player.game.planet.map[mousePos.toVector2Int()];
        }
        if (leftclick)
        {
            player.movePos = mousePos.toVector2Int();
        }
        if (rightclick)
        {
            if (!overlapTile || overlapTile.Revealed)
            {
                player.placeDetector(mousePos.toVector2Int());
            }
            if (overlapTile && !overlapTile.Revealed)
            {
                overlapTile.Flagged = !overlapTile.Flagged;
            }
        }

        //Update player actions
        player.move();
        player.tryReveal();
    }
}
