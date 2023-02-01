using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Grid<T> where T : class
{
    private Dictionary<Vector2Int, T> grid = new Dictionary<Vector2Int, T>();

    #region Mutators
    public void add(Vector2Int v, T t)
    {
        grid[v] = t;
        onGridChanged?.Invoke(this);
    }

    public void remove(Vector2Int v)
    {
        grid.Remove(v);
        onGridChanged?.Invoke(this);
    }

    public delegate void OnGridChanged(Grid<T> grid);
    public event OnGridChanged onGridChanged;
    #endregion

    #region Accessors
    public T get(Vector2Int v)
    {
        if (!grid.ContainsKey(v))
        {
            return null;
        }
        T t = grid[v];
        return t;
    }

    public T this[Vector2Int pos] => get(pos);

    public List<Vector2Int> positions => grid.Keys.ToList();

    public List<T> getNeighbors(Vector2Int v)
    {
        List<T> neighbors = new List<T>();
        for (int x = v.x - 1; x <= v.x + 1; x++)
        {
            for (int y = v.y - 1; y <= v.y + 1; y++)
            {
                Vector2Int neighborV = new Vector2Int(x, y);
                neighbors.Add(get(neighborV));
            }
        }
        return neighbors;
    }

    public Vector2Int Min
    {
        get
        {
            List<Vector2Int> keys = grid.Keys.ToList();
            return new Vector2Int(keys.Min(v => v.x), keys.Min(v => v.y));
        }
    }

    public Vector2Int Max
    {
        get
        {
            List<Vector2Int> keys = grid.Keys.ToList();
            return new Vector2Int(keys.Max(v => v.x), keys.Max(v => v.y));
        }
    }
    #endregion
}
