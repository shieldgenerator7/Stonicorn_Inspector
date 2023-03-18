using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetectorDisplayer : MonoBehaviour
{
    Detector detector;

    public List<Color> colors;

    public Image imgDetector;
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
        //Color
        Color color = colors[Mathf.Clamp(count, 0, colors.Count - 1)];
        imgDetector.color = color;
        txtDetector.color = color;
        //Text
        txtDetector.text = $"{count}";
    }

    private void Update()
    {
        detector.detect();
    }
}
