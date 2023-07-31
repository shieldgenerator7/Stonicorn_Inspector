using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySettings : ScriptableObject
{
    [Tooltip("How fast this entity can move")]
    public float moveSpeed = 1;
}
