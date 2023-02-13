using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2;

    public Vector2 movePos = Vector2.zero;

    public Vector2 Position
    {
        get => transform.position;
        set => transform.position = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        movePos = Position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Position != movePos)
        {
            Vector2 dir = (movePos - Position).normalized;
            Position += dir * Mathf.Min(
                moveSpeed * Time.deltaTime,
                Vector2.Distance(Position, movePos)
                );
            if (Mathf.Approximately(Vector2.Distance(Position,movePos),0))
            {
                Position = movePos;
                onPosReached?.Invoke(Position);
            }
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    movePos = pos;
        //}
    }
    public delegate void OnPosReached(Vector2 pos);
    public event OnPosReached onPosReached;
}
