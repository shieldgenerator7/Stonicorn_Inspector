using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2;

    private Vector2 movePos = Vector2.zero;

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
            Position = Vector2.Lerp(Position, movePos, moveSpeed * Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            movePos = pos;
        }
    }
}
