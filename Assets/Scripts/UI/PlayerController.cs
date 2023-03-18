using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public GameObject detectorPrefab;

    private void Start()
    {
        player.onDetectorAdded += makeDetector;
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
        goDetect.transform.position = (Vector2)detector.Position;
        goDetect.GetComponent<DetectorDisplayer>().init(detector);
    }
}
