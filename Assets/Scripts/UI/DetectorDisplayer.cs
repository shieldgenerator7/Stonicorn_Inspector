using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetectorDisplayer : MonoBehaviour
{
    Detector detector;

    public GameObject goDetector;
    public TMP_Text txtDetector;

    // Start is called before the first frame update
    void Start()
    {
        Planet planet = FindObjectOfType<GameUI>().Game.planet;
        detector = new Detector(planet.map);
        detector.onDetectedAmountChanged += updateDetection;
        detector.detect(transform.position.toVector2Int());
    }

    void updateDetection(int count)
    {
        goDetector.SetActive(count > 0);
        txtDetector.text = $"{count}";
    }
}
