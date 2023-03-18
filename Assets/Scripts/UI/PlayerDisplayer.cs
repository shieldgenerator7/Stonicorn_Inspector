using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PlayerDisplayer : MonoBehaviour
{
    private Player player;
    public GameObject detectorPrefab;

    public DetectorDisplayer followDetectorDisplayer;

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
