using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;

    private void Start()
    {
        if (player == null)
        {
            player = FindAnyObjectByType<GameUI>().Game.player;
        }
        //Autoflag
        player.onPositionChanged += (pos) => tryAutoFlag(pos.toVector2Int(), 2);
        player.OnTileRevealed += (tile, state) => tryAutoFlag(player.Position.toVector2Int(), 2);
        //Stop the player whenever they reveal a tile
        player.OnTileRevealed += stopTask;
    }

    void stopTask(Tile tile = null, bool state = false)
    {
        player.task = Player.Task.NONE;
        player.stop();
        player.autoPlaceDetectors();
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
        Vector2Int mousePosInt = mousePos.toVector2Int();
        bool mouseInRange = player.WithinRangeInt(mousePosInt);
        if (leftclick)
        {
            //Reveal tile without moving
            Tile tile = player.game.planet.map[mousePosInt];
            if (!(tile?.Revealed ?? true) && mouseInRange)
            {
                if (!tile.Flagged)
                {
                    player.revealTile(mousePosInt);
                    stopTask();
                    startTickingTimer();
                }
            }
            //Move to position
            else
            {
                player.task = Player.Task.REVEAL;
                player.MovePosition = mousePosInt;
                timeEnd = 0;
            }
        }

        //Ticking
        if (timeEnd > 0)
        {
            if (Time.time >= timeEnd)
            {
                startTickingTimer(false);
            }
        }

        //Check to close game
        if (Input.GetKeyUp(KeyCode.Escape) && !Input.GetKey(KeyCode.Space))
        {
            Application.Quit();
        }
    }

    public void tryAutoFlag(Vector2Int pos, int range)
    {
        for (int i = pos.x - range; i <= pos.x + range; i++)
        {
            for (int j = pos.y - range; j <= pos.y + range; j++)
            {
                tryAutoFlag(new Vector2Int(i, j));
            }
        }
    }

    public void tryAutoFlag(Vector2Int pos)
    {
        List<Tile> neighbors = player.game.planet.map.getNeighbors(pos);
        List<Tile> hiddenHazardNeigbors = neighbors.FindAll(nghbr => nghbr &&
            !nghbr.Revealed &&
            player.game.enemies.Any(enemy => enemy.Position.toVector2Int() == nghbr.position)
            );
        int hiddenHazardCount = hiddenHazardNeigbors.Count;
        int hiddenSafeCount = neighbors.Count(nghbr => nghbr &&
            !nghbr.Revealed &&
            !player.game.enemies.Any(enemy => enemy.Position.toVector2Int() == nghbr.position));
        bool canAutoFlag = hiddenHazardCount > 0 && hiddenSafeCount == 0
            && hiddenHazardNeigbors.Any(nghbr => !nghbr.Flagged);
        //autoflag
        if (canAutoFlag)
        {
            hiddenHazardNeigbors.ForEach(nghbr => nghbr.Flagged = true);
        }
    }

    float timeEnd = 0;
    void startTickingTimer(bool start = true, float duration = 0.5f)
    {
        timeEnd = (start) ? Time.time + duration : 0;
        player.game.Ticking = start;
    }
}
