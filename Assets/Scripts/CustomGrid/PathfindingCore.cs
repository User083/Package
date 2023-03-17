using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingCore
{
    public List<OverlayInfo> FindPath(OverlayInfo start, OverlayInfo end)
    {
       List<OverlayInfo> openList = new List<OverlayInfo>();
       List<OverlayInfo> closedList = new List<OverlayInfo>();

        openList.Add(start);

        while (openList.Count > 0)
        {
            var selectedTile = openList.OrderBy(x => x.fCost).First();
            

            openList.Remove(selectedTile);
            closedList.Add(selectedTile);

            if (selectedTile == end)
            {
                GetPath(start, end);
            }

            var neighbourTiles = GetNeighbourTiles(selectedTile);

            foreach (var neighbourTile in neighbourTiles)
            {
                if(neighbourTile.isBlocked || closedList.Contains(neighbourTile))
                {
                    continue;
                }

                neighbourTile.gCost = GetManhattenDistance(start, neighbourTile);
                neighbourTile.hCost = GetManhattenDistance(end, neighbourTile);

                neighbourTile.parent = selectedTile;

                if(!openList.Contains(neighbourTile))
                {
                    openList.Add(neighbourTile);
                }
            }
        }

        return new List<OverlayInfo>();

    }

    private int GetManhattenDistance(OverlayInfo start, OverlayInfo neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);
    }

    private List<OverlayInfo> GetPath(OverlayInfo start, OverlayInfo end)
    {
        List<OverlayInfo> path = new List<OverlayInfo>();
        OverlayInfo currentTile = end;

        while(currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Reverse();

        return path;
    }

    private List<OverlayInfo> GetNeighbourTiles(OverlayInfo selectedTile)
    {
        var map = GridManager.Instance.map;

        List<OverlayInfo> neighbours = new List<OverlayInfo>();

        //Top neighbour

        Vector2Int locationCheck = new Vector2Int(selectedTile.gridLocation.x, selectedTile.gridLocation.y + 1);

        if(map.ContainsKey(locationCheck))
        {
            neighbours.Add(map[locationCheck]);
        }

        //Bottom neighbour

        locationCheck = new Vector2Int(selectedTile.gridLocation.x, selectedTile.gridLocation.y - 1);

        if (map.ContainsKey(locationCheck))
        {
            neighbours.Add(map[locationCheck]);
        }

        //Right neighbour

        locationCheck = new Vector2Int(selectedTile.gridLocation.x + 1, selectedTile.gridLocation.y);

        if (map.ContainsKey(locationCheck))
        {
            neighbours.Add(map[locationCheck]);
        }

        //Left neighbour

        locationCheck = new Vector2Int(selectedTile.gridLocation.x - 1, selectedTile.gridLocation.y);

        if (map.ContainsKey(locationCheck))
        {
            neighbours.Add(map[locationCheck]);
        }

        return neighbours;
    }

}
