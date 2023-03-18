using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game 
{
    public Planet planet;

    public Game()
    {
    }

    public void startGame(PlanetGeneration planetGeneration)
    {
        planet = planetGeneration.generatePlanet();
    }//
}
