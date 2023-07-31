using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodController : MonoBehaviour
{
    public float speed = 1;

    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 targetPos;

    public void init(Vector2 podPosition)
    {
        endPos = podPosition;
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
                this.enabled = false;
                onTargetReached?.Invoke(targetPos);
            }
        }
    }

    public void gotoStart()
    {
        targetPos = startPos;
        this.enabled = true;
    }
    public void gotoEnd()
    {
        targetPos = endPos;
        this.enabled = true;
    }
    public delegate void OnTargetReached(Vector2 pos);
    public event OnTargetReached onTargetReached;
}
