using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetGeneration", menuName = "PlanetGeneration")]
public class PlanetGeneration : ScriptableObject
{
    public List<MapGenerator> mapGenerators;

    public Planet generatePlanet()
    {
        Grid<Tile> map = new Grid<Tile>();

        mapGenerators.ForEach(mapGenerator =>
            mapGenerator.generate(map)
        );

        return new Planet(map);
    }
}
