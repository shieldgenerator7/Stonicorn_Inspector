using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;

    private Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        offset = transform.position - follow.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = follow.position + offset;
    }
}
