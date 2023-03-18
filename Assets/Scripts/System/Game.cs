using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game 
{
    public Planet planet;
    public Player player;

    public Game(Player player = null)
    {
        this.player = player ?? new Player();
    }

    public void startGame(PlanetGeneration planetGeneration)
    {
        //Generate planet
        planet = planetGeneration.generatePlanet();
        //Position player
        Vector2 startPos = Vector2.zero;
        player.Position = startPos;
        player.movePos = player.Position;
    }
}
