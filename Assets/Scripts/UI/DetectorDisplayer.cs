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
    public void init(Detector detector)
    {
        this.detector = detector;
        this.detector.onDetectedAmountChanged += updateDetection;
        updateDetection(detector.Detected);
    }

    void updateDetection(int count)
    {
        transform.position = (Vector2)detector.Position;
        goDetector.SetActive(count > 0);
        txtDetector.text = $"{count}";
    }
}
