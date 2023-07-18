using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public PlanetDisplayer planetDisplayer;
    public List<PlanetGeneration> planetGenerationList;

    public GameObject enemyPrefab;//TODO: move this process elsewhere

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
        game.enemies.ForEach(enemy => makeEnemy(enemy));
        //delegates
        this.enabled = false;
        game.onTickingChanged += (ticking) => this.enabled = ticking;
    }

    void makeEnemy(Enemy enemy)
    {
        GameObject goEnemy = Instantiate(enemyPrefab);
        goEnemy.transform.position = enemy.Position;
        goEnemy.GetComponent<EnemyDisplayer>().init(enemy);
    }

    private void Update()
    {
        game.process();
    }
}
