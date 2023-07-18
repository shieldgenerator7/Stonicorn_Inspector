using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;

    private void Start()
    {
        //Stop the player whenever they reveal a tile
        player.OnTileRevealed += (tile, state) => player.MovePosition = player.Position;
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
            Vector2Int mousePosInt = mousePos.toVector2Int();
            //Reveal tile without moving
            Tile tile = player.game.planet.map[mousePosInt];
            if (!(tile?.Revealed ?? true) && Utility.DistanceInt(player.Position.toVector2Int(), mousePosInt) <= player.inspectRange)
            {
                player.revealTile(mousePosInt);
            }
            //Move to position
            else
            {
                player.MovePosition = mousePosInt;
            }
        }
        if (rightclick)
        {
            if (!overlapTile || overlapTile.Revealed)
            {
            }
            if (overlapTile && !overlapTile.Revealed)
            {
                overlapTile.Flagged = !overlapTile.Flagged;
            }
        }

        //Check to close game
        if (Input.GetKeyUp(KeyCode.Escape) && !Input.GetKey(KeyCode.Space))
        {
            Application.Quit();
        }
    }
}
