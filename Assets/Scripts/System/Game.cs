using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Game 
{
    public Planet planet = new Planet();

    public Game()
    {
        planet = new Planet();
    }
}
