using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindingCore
{
    public List<OverlayInfo> FindPath(OverlayInfo start, OverlayInfo end, List<OverlayInfo> inRangeTiles)
    {
       List<OverlayInfo> openList = new List<OverlayInfo>();
       List<OverlayInfo> closedList = new List<OverlayInfo>();
       List<OverlayInfo> path = new List<OverlayInfo>();
   
        openList.Add(start);

        while (openList.Count() > 0)
        {
            var selectedTile = openList.OrderBy(x => x.fCost).First();
            

            openList.Remove(selectedTile);
            closedList.Add(selectedTile);

            if (selectedTile == end)
            {
                path = GetPath(start, end, inRangeTiles);
                
            }

            var neighbourTiles = GridManager.Instance.GetNeighbourTiles(selectedTile, inRangeTiles);

            foreach (var neighbourTile in neighbourTiles)
            {
                if(neighbourTile.isBlocked || closedList.Contains(neighbourTile))
                {
                    continue;
                }

                //neighbourTile.gCost = GetManhattenDistance(start, neighbourTile);
                neighbourTile.gCost = GetEuclideanDistance(start, neighbourTile);
                neighbourTile.setStatus();
                
                //neighbourTile.hCost = GetManhattenDistance(end, neighbourTile);
                neighbourTile.hCost = GetEuclideanDistance(end, neighbourTile);

                neighbourTile.parent = selectedTile;

                if(!openList.Contains(neighbourTile))
                {
                    openList.Add(neighbourTile);
                }
            }
        }

        path.Reverse();

       return path;

    }

    private int GetManhattenDistance(OverlayInfo start, OverlayInfo neighbour)
    {
        return Mathf.Abs(start.gridLocation.x - neighbour.gridLocation.x) + Mathf.Abs(start.gridLocation.y - neighbour.gridLocation.y);

    }

    private int GetEuclideanDistance(OverlayInfo start, OverlayInfo neighbour)
    {
        float difference = Mathf.Sqrt(Mathf.Abs(Magnitude(start.gridLocation2D - neighbour.gridLocation2D)));
           
        
        return Mathf.RoundToInt(difference);
    }

    public float Magnitude(Vector2 a)
    {
        if (a.GetType() == typeof(Vector2))
        {
            return Mathf.Sqrt((a.x * a.x) + (a.y * a.y));
        }
        else
        {
            return 0;
        }

    }

    private List<OverlayInfo> GetPath(OverlayInfo start, OverlayInfo end, List<OverlayInfo> inRangeTiles)
    {
        List<OverlayInfo> path = new List<OverlayInfo>();
        OverlayInfo currentTile = end;
       
             while(currentTile != start)
                    {
                        path.Add(currentTile);
                        currentTile = currentTile.parent;
                    }      


        return path;
    }


}
