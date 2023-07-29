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
    }

    // Update is called once per frame
    void Update()
    {
        //If player returned to startpos
        if (player.Position == startpos)
        {
            onPlayerFinished?.Invoke();
        }
    }
    public delegate void OnPlayerFinished();
    public event OnPlayerFinished onPlayerFinished;
}
