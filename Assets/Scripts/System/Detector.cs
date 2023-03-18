using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Detector
{
    private int range;
    public int Range => range;

    private Vector2Int pos;
    public Vector2Int Position => pos;

    private int detectedAmount;
    public int Detected
    {
        get => detectedAmount;
        private set
        {
            detectedAmount = value;
            onDetectedAmountChanged?.Invoke(detectedAmount);
        }
    }
    public delegate void OnDetectedAmountChanged(int amount);
    public event OnDetectedAmountChanged onDetectedAmountChanged;

    private Grid<Tile> map;

    public Detector(Grid<Tile> map, int range = 1)
    {
        this.map = map;
        this.range = range;
    }

    public void detect()
    {
        detect(pos);
    }

    public void detect(Vector2Int pos)
    {
        this.pos = pos;
        Detected = map.getNeighbors(pos, range)
            .Count(t => t && t.objects.Count > 0);
    }
}
