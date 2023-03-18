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
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    movePos = pos;
        //}

        //Update player actions
        player.move();
        bool tileRevealed = player.tryReveal();
        if (tileRevealed)
        {
            player.placeDetector(player.movePos.toVector2Int());
        }
    }
}
