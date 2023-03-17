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

        while (openList.Count > 0)
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

                neighbourTile.gCost = GetManhattenDistance(start, neighbourTile);
                neighbourTile.hCost = GetManhattenDistance(end, neighbourTile);

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
