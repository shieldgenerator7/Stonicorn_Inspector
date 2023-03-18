using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public GameObject detectorPrefab;

    private Detector followDetector;
    private DetectorDisplayer followDetectorDisplayer;

    private void Start()
    {
        player.onDetectorAdded += (detector) => makeDetector(detector);
        //Make follow detector
        followDetector = new Detector(player.game.planet.map, 2);
        followDetectorDisplayer = makeDetector(followDetector);
        followDetector.detect(player.Position.toVector2Int());
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    movePos = pos;
        //}

        //Update player actions
        player.move();
        Vector2 pos = player.Position;
        followDetector.detect(pos.toVector2Int());
        bool tileRevealed = player.tryReveal();
        if (tileRevealed)
        {
            player.placeDetector(player.movePos.toVector2Int());
        }
        //Update display
        transform.position = pos;
        followDetectorDisplayer.transform.position = pos;
    }

    public DetectorDisplayer makeDetector(Detector detector)
    {
        GameObject goDetect = Instantiate(detectorPrefab);
        goDetect.transform.position = (Vector2)detector.Position;
        DetectorDisplayer detectorDisplayer = goDetect.GetComponent<DetectorDisplayer>();
        detectorDisplayer.init(detector);
        return detectorDisplayer;
    }
}
