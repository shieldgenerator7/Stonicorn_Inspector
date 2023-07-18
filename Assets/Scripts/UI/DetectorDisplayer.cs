using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetectorDisplayer : MonoBehaviour
{
    Detector detector;

    public List<Color> colors;

    public GameObject goDetector;
    public Image imgDetector;
    public TMP_Text txtDetector;

    // Start is called before the first frame update
    public void init(Detector detector)
    {
        this.detector = detector;
        this.detector.onDetectedAmountChanged += updateDetection;
        updateDetection(detector.Detected);
        this.detector.onDestroyed += () => Destroy(gameObject);
    }

    public Color getColor(int count)
    {
        return colors[Mathf.Clamp(count, 0, colors.Count - 1)];
    }

    void updateDetection(int count)
    {
        //Visible
        goDetector.SetActive(count > 0 || detector.game.player.DetectorCount > 0);
        //Color
        Color color = getColor(count);
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
