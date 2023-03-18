using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public PlanetDisplayer planetDisplayer;
    public List<PlanetGeneration> planetGenerationList;

    [SerializeField]
    private Game game;
    public Game Game => game;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game();
        game.startGame(planetGenerationList.randomItem());
        planetDisplayer.init(game.planet);
        FindObjectOfType<PlayerController>().player = game.player;
        FindObjectOfType<PlayerDisplayer>().init(game.player);
    }
}
