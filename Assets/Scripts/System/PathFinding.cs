using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//2023-07-31: made following a tutorial: https://web.archive.org/web/20170505034417/http://blog.two-cats.com/2014/06/a-star-example/
public class PathFinding
{
    enum NoteState
    {
        Untested,
        Opened,
        Closed
    }
    class PathNode
    {
        //settings
        public bool isWalkable;
        public Vector2Int position;
        //processing info
        public PathNode parent = null;
        public int pathLength = 0;
        public int goalDistance = 0;
        public int EstimatedDistance => pathLength + goalDistance;
        public NoteState state = NoteState.Untested;

        public bool Searachable => isWalkable && state == NoteState.Opened;

        public PathNode(bool walkable = true)
        {
            this.isWalkable = walkable;
        }

        public static implicit operator bool(PathNode node) => node != null;
    }
    public static List<Vector2Int> FindPath(Vector2 start, Vector2 end, Grid<bool> map1)
    {
        return FindPath(start.toVector2Int(), end.toVector2Int(), map1);
    }
    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int end, Grid<bool> map1)
    {
        //case: start not walkable
        if (!map1[start])
        {
            return null;
        }
        //case: end not walkable
        if (!map1[end])
        {
            return null;
        }
        //
        List<PathNode> searchPositions;
        Grid<PathNode> map = map1.Map((b) => new PathNode(b));
        map.positions.ForEach(position => map[position].position = position);
        map[start].state = NoteState.Closed;
        //map[end].state = NoteState.Closed;
        //
        searchPositions = map.getNeighbors(start, 1)
            .FindAll(pn => pn.Searachable);
        searchPositions.ForEach(pn =>
        {
            pn.parent = map[start];
            pn.pathLength = 1;
        });
        searchPositions.Sort((a, b) => a.EstimatedDistance - b.EstimatedDistance);
        while (searchPositions.Count > 0)
        {
            //search
            PathNode node = searchPositions[0];
            node.state = NoteState.Closed;
            if (node.position == end)
            {
                break;
            }
            List<PathNode> search = map.getNeighbors(node.position, 1)
                .FindAll(pn => pn.Searachable);
            search.ForEach(pn =>
            {
                pn.parent = node;
                pn.pathLength = node.pathLength + 1;
                pn.goalDistance = Utility.DistanceInt(node.position, end);
            });

            searchPositions.RemoveAt(0);
            searchPositions.AddRange(search);
            searchPositions.Sort((a, b) => a.EstimatedDistance - b.EstimatedDistance);
        }
        //
        PathNode endNode = map[end];
        //case: no path was found
        if (!endNode.parent)
        {
            return null;
        }
        //case: path was found
        List<Vector2Int> path = new List<Vector2Int>();
        while (endNode.parent)
        {
            path.Add(endNode.position);
            endNode = endNode.parent;
        }
        path.Add(start);
        path.Reverse();
        return path;
    }
}
