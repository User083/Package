using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public OverlayInfo overlayTileInfo;
    public GameObject overlayContainer;
    private BoundsInt bounds;
    public Tilemap tileMap;
    public Dictionary<Vector2Int, OverlayInfo> map;
    public Dictionary<OverlayInfo, TileBase> tileTypes;
    private List<OverlayInfo> overlays;

    public void GenerateGrid()
    {
        map = new Dictionary<Vector2Int, OverlayInfo>();
        tileTypes = new Dictionary<OverlayInfo, TileBase>();

        bounds = tileMap.cellBounds;


        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);



                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {

                        var overlayTile = Instantiate(overlayTileInfo, overlayContainer.transform);
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);
                        TileBase tileType = tileMap.GetTile(tileLocation);


                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                        overlayTile.gridLocation = tileLocation;


                        map.Add(tileKey, overlayTile);
                        tileTypes.Add(overlayTile, tileType);

                    }
                }
            }
        }

        SetTileStatus();
        overlays = map.Values.ToList();
    }

    public void SetTileStatus()
    {
      
        foreach(KeyValuePair<OverlayInfo, TileBase> item in tileTypes)
        {
         

            if(item.Value.ToString().Contains("obst"))
            {
                item.Key.setBlocked(true);
            }
            else if(item.Value.ToString().Contains("goal"))
            {
                item.Key.setEnd(true);
                GameManager.Instance.endTile = item.Key;
            }
            else if (item.Value.ToString().Contains("start"))
            {
                item.Key.setStart(true);
                GameManager.Instance.startTile = item.Key;
            }
            else if (item.Value.ToString().Contains("trap"))
            {
                item.Key.hasTrap= true;
            }
            else if (item.Value.ToString().Contains("tree"))
            {
                item.Key.isTree = true;
            }
            else if (item.Value.ToString().Contains("road"))
            {
                item.Key.isRoad = true;
            }
            else if (item.Value.ToString().Contains("water"))
            {
                item.Key.setBlocked(true);
            }
            else if (item.Value.ToString().Contains("NoSpawn"))
            {
                item.Key.noSpawn = true;
            }



        }
    }

    public OverlayInfo GetRandomTile()
    {
        
        List<OverlayInfo> tempList = new List<OverlayInfo>();
        
        foreach (OverlayInfo tile in overlays)
        {
            if(!tile.isBlocked && !tile.hasTrap)
            {
                tempList.Add(tile);
            }
        }
        if (tempList.Count() > 0)
        {
            int i = Random.Range(0, tempList.Count() - 1);

            return tempList.ElementAt(i);
        }
        else
        {
            Debug.LogWarning("No random tile available");
            return null;
        }
    }




    public OverlayInfo GetRandomSpawnTile()
    {

        List<OverlayInfo> tempList = new List<OverlayInfo>();

        foreach (OverlayInfo tile in overlays)
        {
            if (!tile.isBlocked && !tile.noSpawn && !tile.hasTrap)
            {
                tempList.Add(tile);
            }
        }
        int i = Random.Range(0, tempList.Count());

        return tempList.ElementAt(i);
    }

    public OverlayInfo GetRandomSpawnRangeTile(List<OverlayInfo> tiles)
    {

        List<OverlayInfo> tempList = new List<OverlayInfo>();

        foreach (OverlayInfo tile in overlays)
        {
            if (!tile.isBlocked && !tile.noSpawn && !tiles.Contains(tile) && !tile.hasTrap)
            {
                tempList.Add(tile);
            }
        }

        if(tempList.Count > 0)
        {
            int i = Random.Range(0, tempList.Count());

            return tempList.ElementAt(i);
        }

        return null;
    }

    public void ResetAllOverlays()
    {
        foreach(var overlay in overlays)
        {
            overlay.HideTile();
            if(overlay.hasEnemy)
            {
                overlay.isBlocked= false;
                overlay.hasEnemy=false;
            }
        }
    }

    public List<OverlayInfo> GetNeighbourTiles(OverlayInfo selectedTile, List<OverlayInfo> inRangeTiles)
    {

        Dictionary<Vector2Int, OverlayInfo> rangeTiles = new Dictionary<Vector2Int, OverlayInfo>();

        if(inRangeTiles.Count() > 0)
        {
            foreach(var item in inRangeTiles)
            {
                rangeTiles.Add(item.gridLocation2D, item);
            }
        }
        else
        {
            rangeTiles = map;
        }
        List<OverlayInfo> neighbours = new List<OverlayInfo>();

        //Top neighbour

        Vector2Int locationCheck = new Vector2Int(selectedTile.gridLocation.x, selectedTile.gridLocation.y + 1);
            
        
        if (rangeTiles.ContainsKey(locationCheck))
        {
            if (Mathf.Abs(selectedTile.gridLocation.z - rangeTiles[locationCheck].gridLocation.z) <= 1)
             neighbours.Add(rangeTiles[locationCheck]);
        }

        //Bottom neighbour

        locationCheck = new Vector2Int(selectedTile.gridLocation.x, selectedTile.gridLocation.y - 1);

        if (rangeTiles.ContainsKey(locationCheck))
        {
            if (Mathf.Abs(selectedTile.gridLocation.z - rangeTiles[locationCheck].gridLocation.z) <= 1)
                neighbours.Add(rangeTiles[locationCheck]);
        }

        //Right neighbour

        locationCheck = new Vector2Int(selectedTile.gridLocation.x + 1, selectedTile.gridLocation.y);

        if (rangeTiles.ContainsKey(locationCheck))
        {
            if (Mathf.Abs(selectedTile.gridLocation.z - rangeTiles[locationCheck].gridLocation.z) <= 1)
                neighbours.Add(rangeTiles[locationCheck]);
        }

        //Left neighbour

        locationCheck = new Vector2Int(selectedTile.gridLocation.x - 1, selectedTile.gridLocation.y);

        if (rangeTiles.ContainsKey(locationCheck))
        {
            if (Mathf.Abs(selectedTile.gridLocation.z - rangeTiles[locationCheck].gridLocation.z) <= 1)
                neighbours.Add(rangeTiles[locationCheck]);
        }

        return neighbours;
    }
}
