using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingCharacter : MonoBehaviour
{
    [Header("Generic Traits")]
    public float speed = 4f;
    public int range = 3;
    public GameObject prefab;
    private SpriteRenderer spriteRenderer;
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
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        activeTile = tile;

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
        if(!activeTile.isEnd)
        {
            activeTile.isBlocked = state;
            activeTile.hasEnemy = state;
        }
        
    }
}
