using System;
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

    /// <summary>
    /// Returns a list of all objects in the grid
    /// within the given range of the given coordinate, including the coordinate
    /// </summary>
    /// <param name="v"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public List<T> getNeighbors(Vector2Int v, int range = 1)
    {
        List<T> neighbors = new List<T>();
        for (int x = v.x - range; x <= v.x + range; x++)
        {
            for (int y = v.y - range; y <= v.y + range; y++)
            {
                Vector2Int neighborV = new Vector2Int(x, y);
                neighbors.Add(get(neighborV));
            }
        }
        return neighbors;
    }

    public int getNeighborCount(Vector2Int v, int range = 1, Func<T, bool> filter = null)
    {
        List<T> neighbors = getNeighbors(v, range);
        if (filter != null)
        {
            return neighbors.Count(filter);
        }
        return neighbors.Count;
    }

    public List<T> FindAll(Func<T, bool> filter)
        => grid.Values.ToList().FindAll((val) => filter(val)).ToList();

    public bool Any(Func<T, bool> filter)
        => grid.Values.Any((val) => filter(val));

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
