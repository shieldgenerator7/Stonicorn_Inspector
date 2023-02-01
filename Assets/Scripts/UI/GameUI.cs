using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public PlanetDisplayer planetDisplayer;
    public List<MapGenerator> mapGenerators;

    [SerializeField]
    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game();
        planetDisplayer.init(game.planet);
        mapGenerators.ForEach(mapGenerator => 
            game.planet.generate(mapGenerator)
        );
    }
}
