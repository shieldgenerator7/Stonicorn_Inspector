using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public GameObject detectorPrefab;

    private Detector followDetector;

    private void Start()
    {
        player.onDetectorAdded += makeDetector;
        followDetector = player.placeDetector(player.Position.toVector2Int(), 2);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    movePos = pos;
        //}

        player.move();
        followDetector.detect(player.Position.toVector2Int());
        bool tileRevealed = player.tryReveal();
        if (tileRevealed)
        {
            player.placeDetector(player.movePos.toVector2Int());
        }
        transform.position = player.Position;
    }

    public void makeDetector(Detector detector)
    {
        GameObject goDetect = Instantiate(detectorPrefab);
        goDetect.GetComponent<DetectorDisplayer>().init(detector);
    }
}
