using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PodDisplayer : MonoBehaviour
{
    Vector2 startpos;
    Player player;
    Game game;

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<GameUI>().Game;
        player = game.player;
        startpos = player.Position;
        transform.position = startpos;
    }

    // Update is called once per frame
    void Update()
    {
        //If player returned to startpos
        if (player.Position == startpos)
        {
            bool anySafeUnrevealed = game.planet.map.Any(
                tile => !tile.Revealed && !game.enemies.Any(
                    enemy => enemy.Position == tile.position
                    )
                );
            if (!anySafeUnrevealed)
            {
                //Win the game
                FindObjectsOfType<EnemyDisplayer>().ToList().ForEach(
                    enemyDisplayer => Destroy(enemyDisplayer)
                    );
                player.moveSpeed = 0;
                FindObjectsOfType<DetectorDisplayer>().ToList().ForEach(
                    enemyDisplayer => Destroy(enemyDisplayer)
                    );

            }
        }
    }
}
