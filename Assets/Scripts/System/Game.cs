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
        this.player.game = this;
    }

    public void startGame(PlanetGeneration planetGeneration)
    {
        //Generate planet
        planet = planetGeneration.generatePlanet();
        //Position player
        Vector2Int min = planet.map.Min;
        Vector2Int max = planet.map.Max;
        Vector2 startPos = new Vector2((min.x + max.x) / 2, max.y + 2);
        player.Position = startPos;
        player.movePos = player.Position;
    }
}
