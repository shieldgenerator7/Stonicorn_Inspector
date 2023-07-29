using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float speed = 1;

    public Vector2 startPos;
    public Vector2 endPos;

    [SerializeField]
    private Vector2 targetPos;

    private void Start()
    {
        Game game = FindObjectOfType<GameUI>().Game;
        endPos = game.player.Position;
        startPos = endPos + (Vector2.up * 10);
        transform.position = startPos;
    }

    private void Update()
    {
        if ((Vector2)transform.position != targetPos)
        {
            transform.position = Vector2.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, targetPos) < 0.25f)
            {
                transform.position = targetPos;
                onTargetReached?.Invoke(targetPos);
            }
        }
    }

    public void gotoStart()
    {
        targetPos = startPos;
    }
    public void gotoEnd()
    {
        targetPos = endPos;
    }
    public delegate void OnTargetReached(Vector2 pos);
    public event OnTargetReached onTargetReached;
}
