using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingCharacter : MonoBehaviour
{
    [Header("Generic Traits")]
    public float speed = 4f;
    public int range;
    public GameObject prefab;
    public SpriteRenderer spriteRenderer;
    public bool isPlayer = false;
    public bool playerTurn;

    [Header ("Pathfinding")]
    private RangeFinder rangeFinder;
    protected PathfindingCore pathFinder;
    public List<OverlayInfo> path = new List<OverlayInfo>();
    public List<OverlayInfo> inRangeTiles = new List<OverlayInfo>();
    public OverlayInfo activeTile = null;
    public bool isAttacking;
    
    private void Awake()
    {
        pathFinder = new PathfindingCore();
        rangeFinder = new RangeFinder();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }


    //Calculate range of tiles character can move
    public void CalculateRange()
    {
        if (inRangeTiles.Count > 0)
        {
            foreach (var item in inRangeTiles)
            {
                item.HideTile();
            }
        }


        inRangeTiles = rangeFinder.GetTilesInRange(activeTile, range);

        if (inRangeTiles.Count > 0)
        {
            foreach (var item in inRangeTiles)
            {
                if(isPlayer)
                {
                    item.ShowTile();
                }
                else
                {
                    item.ShowEnemyTile();
                }
                
            }
        }
    }



    public void PositionCharacter(OverlayInfo tile)
    {
        var newTile = tile;
        if(tile == null)
        {
            newTile = GameManager.Instance.gridManager.GetRandomSpawnTile();
            Debug.LogWarning("No position tile - random tile generated");
        }
        transform.position = new Vector3(newTile.transform.position.x, newTile.transform.position.y + 0.0001f, newTile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = newTile.GetComponent<SpriteRenderer>().sortingOrder;
        activeTile = newTile;

        if (activeTile == GameManager.Instance.endTile && isPlayer)
        {
            GameManager.Instance.endTileReached= true;
        }

           updateActiveTile(true);

    }

    public void SpriteDirection(OverlayInfo destination)
    {
        if (destination.transform.position.x > activeTile.transform.position.x)
            spriteRenderer.flipX = false;
        else if (destination.transform.position.x < activeTile.transform.position.x)
            spriteRenderer.flipX = true;
    }

    public void updateActiveTile(bool state)
    {
        if(!activeTile.isEnd && !activeTile.hasPackage)
        {
            activeTile.isBlocked = state;
            activeTile.hasEnemy = state;
        }
        if (!state)
        {
            activeTile.activeEnemy = null;
        }
        
    }

    protected OverlayInfo GetRandomTileInRange(List<OverlayInfo> list)
    {
        List<OverlayInfo> tempList = new List<OverlayInfo>();

        foreach (OverlayInfo tile in list)
        {
            if (!tile.isBlocked)
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
}
