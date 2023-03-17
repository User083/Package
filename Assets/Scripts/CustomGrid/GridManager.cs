using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;

    public static GridManager Instance { get { return instance; } }

    public OverlayInfo overlayTileInfo;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayInfo> map;
    private void Awake()
    {
        if(instance != null && instance !=this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, OverlayInfo>();

        BoundsInt bounds = tileMap.cellBounds;

        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);

                    if(tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {
                        var overlayTile = Instantiate(overlayTileInfo, overlayContainer.transform);
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                        //overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                        overlayTile.gridLocation = tileLocation;

                        map.Add(tileKey, overlayTile);
                    }
                }
            }
        }
    }
}
