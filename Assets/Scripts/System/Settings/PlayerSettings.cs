using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Settings/Player")]
public class PlayerSettings : EntitySettings
{
    [Tooltip("How far away the player can see from themselves")]
    /// <summary>
    /// How far away the player can see from themselves
    /// </summary>
    public int visionRange = 5;

    [Tooltip("How close the player must be to reveal a tile")]
    /// <summary>
    /// How close the player must be to reveal a tile
    /// </summary>
    public int inspectRange = 2;
}
