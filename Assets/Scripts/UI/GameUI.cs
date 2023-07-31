using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public PlanetDisplayer planetDisplayer;
    public StonicornSettings playerSettings;
    public List<PlanetGeneration> planetGenerationList;
    public GameObject allInspected;

    public GameObject enemyPrefab;//TODO: move this process elsewhere

    [SerializeField]
    private Game game;
    public Game Game => game;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game(playerSettings);
        game.startGame(planetGenerationList.randomItem());
        planetDisplayer.init(game.planet);
        PlayerController pc = FindObjectOfType<PlayerController>();
        pc.player = game.player;
        PlayerDisplayer pd = FindObjectOfType<PlayerDisplayer>();
        pd.init(game.player);
        pd.Update();
        game.enemies.ForEach(enemy => makeEnemy(enemy));
        //delegates
        this.enabled = false;
        game.onTickingChanged += (ticking) => this.enabled = ticking;
        //Pod
        game.player.moveSpeed = 0;
        PodController sc = FindObjectOfType<PodController>();
        sc.init(game.podPosition);
        pc.transform.parent = sc.transform;
        pc.transform.localPosition = Vector2.zero;
        pd.enabled = false;
        sc.gotoEnd();
        sc.onTargetReached += (pos) =>
        {
            pd.enabled = true;
            game.player.moveSpeed = 2;
            pc.transform.parent = null;
        };
        game.onGameEnded += () =>
        {
            sc.gotoStart();
            sc.onTargetReached += (pos) =>
            {
                SceneManager.LoadScene(0);
            };

            pd.enabled = false;
            pc.transform.parent = sc.transform;
            pc.transform.localPosition = Vector2.zero;
            //Win the game
            FindObjectsOfType<EnemyDisplayer>().ToList().ForEach(
                enemyDisplayer => Destroy(enemyDisplayer)
                );
            game.player.moveSpeed = 0;
            FindObjectsOfType<DetectorDisplayer>().ToList().ForEach(
                detectorDisplayer => Destroy(detectorDisplayer.gameObject)
                );

            allInspected.SetActive(false);
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
