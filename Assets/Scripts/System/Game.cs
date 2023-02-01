using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game 
{
    public int radius = 5;
    public Planet planet = new Planet();

    public Game()
    {
        planet = new Planet();
        PlanetMapGenerator<Tile> pmg = new PlanetMapGenerator<Tile>();
        pmg.radius = radius;
        pmg.generate(planet.map);
    }
}
