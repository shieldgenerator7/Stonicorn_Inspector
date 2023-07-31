using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public PlanetDisplayer planetDisplayer;
    public List<PlanetGeneration> planetGenerationList;
    public GameObject allInspected;

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
        PlayerController pc = FindObjectOfType<PlayerController>();
        pc.player = game.player;
        FindObjectOfType<PlayerDisplayer>().init(game.player);

        FindObjectOfType<PlayerDisplayer>().Update();
        game.enemies.ForEach(enemy => makeEnemy(enemy));
        //delegates
        this.enabled = false;
        game.onTickingChanged += (ticking) => this.enabled = ticking;
        //Ship
        game.player.moveSpeed = 0;
        PodController sc = FindObjectOfType<PodController>();
        pc.transform.parent = sc.transform;
        pc.transform.localPosition = Vector2.zero;
        FindObjectOfType<PlayerDisplayer>().enabled = false;
        sc.gotoEnd();
        sc.onTargetReached += (pos) =>
        {
            FindObjectOfType<PlayerDisplayer>().enabled = true;
            game.player.moveSpeed = 2;
            pc.transform.parent = null;
        };
        FindObjectOfType<PodDisplayer>().onPlayerFinished += () =>
        {
            bool allTilesRevealed = game.planet.map.All(tile => tile.Revealed || tile.Flagged);
            if (allTilesRevealed)
            {
                sc.gotoStart();
                sc.onTargetReached += (pos) =>
                {
                    SceneManager.LoadScene(0);
                };

                FindObjectOfType<PlayerDisplayer>().enabled = false;
                pc.transform.parent = sc.transform;
                //Win the game
                FindObjectsOfType<EnemyDisplayer>().ToList().ForEach(
                    enemyDisplayer => Destroy(enemyDisplayer)
                    );
                game.player.moveSpeed = 0;
                FindObjectsOfType<DetectorDisplayer>().ToList().ForEach(
                    detectorDisplayer => Destroy(detectorDisplayer.gameObject)
                    );

                allInspected.SetActive(false);
            }
        };
        pc.player.OnTileRevealed += (tile, state) =>
        {
            bool allTilesRevealed = game.planet.map.All(tile => tile.Revealed || tile.Flagged);
            if (allTilesRevealed)
            {
                allInspected.SetActive(true);
            }
        };
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
