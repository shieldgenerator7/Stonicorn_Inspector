using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "PlanetGeneration", menuName = "PlanetGeneration")]
public class PlanetGeneration : ScriptableObject
{
    public List<MapGenerator> mapGenerators;
    public List<EnemySettings> enemies;

    public Planet generatePlanet()
    {
        Grid<Tile> map = new Grid<Tile>();

        mapGenerators.ForEach(mapGenerator =>
            mapGenerator.generate(map)
        );

        return new Planet(map);
    }

    public EnemySettings getRandomEnemy()
    {
        return enemies[Random.Range(0, enemies.Count)];
    }
}
