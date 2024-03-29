using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisplayer : MonoBehaviour
{
    public Enemy enemy;

    // Start is called before the first frame update
    public void init(Enemy enemy)
    {
        this.enemy = enemy;
        enemy.onPositionChanged += updatePosition;
    }

    void updatePosition(Vector2 pos)
    {
        transform.position = pos;
    }
}
