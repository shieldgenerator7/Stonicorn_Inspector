using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Game
{
    public Planet planet;
    public Player player;
    public List<Enemy> enemies = new List<Enemy>();
    public Vector2Int podPosition;

    private bool ticking = false;//whether or not time is progressing forward
    public bool Ticking
    {
        get => ticking;
        set
        {
            ticking = value;
            onTickingChanged?.Invoke(ticking);
        }
    }
    public delegate void OnTickingChanged(bool ticking);
    public event OnTickingChanged onTickingChanged;

    public Game(Player player = null)
    {
        this.player = player ?? new Player(this);
        this.player.onMovePositionChanged += (pos) => Ticking = true;
        this.player.onPosReached += (pos) => Ticking = false;
    }

    public void startGame(PlanetGeneration planetGeneration)
    {
        //Generate planet
        planet = planetGeneration.generatePlanet();
        //Replace hazards with enemies
        enemies.Clear();
        List<Tile> hazardTiles = planet.map.FindAll(
            tile => tile.objects.Any(obj => obj is Hazard)
            );
        hazardTiles.ForEach(tile =>
        {
            Enemy enemy = new Enemy(this);
            enemy.init(tile.position);
            enemies.Add(enemy);
            tile.objects.RemoveAll(obj => obj is Hazard);
        });
        //Position player
        Vector2Int min = planet.map.Min;
        Vector2Int max = planet.map.Max;
        Vector2 startPos = new Vector2((min.x + max.x) / 2, max.y + 3);
        podPosition = startPos.toVector2Int();
        player.Position = podPosition;
        player.MovePosition = podPosition;
        //Init player
        player.init(planet);
    }

    public void process()
    {
        player.process();
        enemies.ForEach(enemy => enemy.process());
    }
}
