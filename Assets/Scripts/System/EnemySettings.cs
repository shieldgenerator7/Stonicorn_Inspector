using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "Settings/Enemy")]
public class EnemySettings : EntitySettings
{
    [Tooltip("How fast it can move when not chasing")]
    public float wanderMoveSpeed = 0.5f;
    [Tooltip("How close it must be to the player to chase")]
    public float seekRange = 1;
    [Tooltip("How far from its start position it can wander without chasing")]
    public float tetherRange = 1;
}
