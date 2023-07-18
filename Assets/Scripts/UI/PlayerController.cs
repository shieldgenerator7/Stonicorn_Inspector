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
            player.MovePosition = mousePos.toVector2Int();
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
