using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisplayer : MonoBehaviour
{
    private Player player;
    public GameObject detectorPrefab;

    public DetectorDisplayer followDetectorDisplayer;
    public SpriteRenderer scanSquare;

    // Start is called before the first frame update
    public void init(Player player)
    {
        this.player = player;
        this.player.onDetectorAdded += (detector) => makeDetector(detector);
        //Make follow detector
        followDetectorDisplayer.init(player.FollowDetector);
    }

    // Update is called once per frame
    void Update()
    {
        //Update display
        Vector2 pos = player.Position;
        transform.position = pos;
        //Scan Square
        scanSquare.transform.position = (Vector2)player.FollowDetector.Pos;
        Color color = followDetectorDisplayer.getColor(player.FollowDetector.Detected);
        color.a = scanSquare.color.a;
        scanSquare.color = color;
        scanSquare.enabled = followDetectorDisplayer.goDetector.activeSelf;
    }

    public DetectorDisplayer makeDetector(Detector detector)
    {
        GameObject goDetect = Instantiate(detectorPrefab);
        goDetect.transform.position = (Vector2)detector.Pos;
        DetectorDisplayer detectorDisplayer = goDetect.GetComponent<DetectorDisplayer>();
        detectorDisplayer.init(detector);
        return detectorDisplayer;
    }
}
