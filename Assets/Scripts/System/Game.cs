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
    public Vector2Int podPosition { get; private set; }

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

    public Game(StonicornSettings playerSettings)
    {
        this.player = new Player(this, playerSettings);
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
            Enemy enemy = new Enemy(this, planetGeneration.getRandomEnemy());
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
        //Delegates
        player.OnTileRevealed += (tile, state) => checkGameEnd();
        player.OnTileFlagged += (tile, state) => checkGameEnd();
        player.onPosReached += (pos) => checkGameEnd();
    }

    public void process()
    {
        player.process();
        enemies.ForEach(enemy => enemy.process());
    }

    private void checkGameEnd()
    {
        bool allTilesRevealed = planet.map.All(tile => tile.Revealed || tile.Flagged);
        bool playerInPod = player.Position == podPosition;
        if (allTilesRevealed && playerInPod)
        {
            onGameEnded?.Invoke();
        }
    }
    public delegate void OnGameEnded();
    public event OnGameEnded onGameEnded;
}
