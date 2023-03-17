using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder
{
public List<OverlayInfo> GetTilesInRange(OverlayInfo startTile, int range)
    {
        var inRangeTiles = new List<OverlayInfo>();
        int stepCount = 0;

        inRangeTiles.Add(startTile);

        var tileForPreviousStep = new List<OverlayInfo>();
        tileForPreviousStep.Add(startTile);

        while(stepCount < range) 
        { 
            var surroundingTiles = new List<OverlayInfo>();

            foreach(var item in tileForPreviousStep)
            {
                surroundingTiles.AddRange(GridManager.Instance.GetNeighbourTiles(item, new List<OverlayInfo>()));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        
        }

        return inRangeTiles.Distinct().ToList();
    }
}
