using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingCharacter : MonoBehaviour
{
    private RangeFinder rangeFinder;
    private PathfindingCore pathFinder;
    public List<OverlayInfo> path = new List<OverlayInfo>();
    public List<OverlayInfo> inRangeTiles = new List<OverlayInfo>();
    public List<OverlayInfo> pathToEnd = new List<OverlayInfo>();
    public OverlayInfo activeTile = null;
    public float speed = 4f;
    public int range = 3;
    public GameObject prefab;
    private SpriteRenderer renderer;
    public bool isPlayer = false;
    public bool playerTurn;
    public bool isMoving;


    private void Awake()
    {
        pathFinder = new PathfindingCore();
        rangeFinder = new RangeFinder();
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
     

    }
    private void Start()
    {
        

    }
    public void MoveTo()
    {
        updateActiveTile(false);
        isMoving = true;
        var step = speed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);


        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.000001f)
        {
            PositionCharacter(path[0]);
            path.RemoveAt(0);
            isMoving = false;
        }

        if (path.Count == 0)
        {
          
            CalculateRange();
            GameManager.Instance.endEnemyTurn();
            isMoving= false;    

        }

    }

    public void MovePlayerTo()
    {
        updateActiveTile(false);
        isMoving = true;
        var step = speed * Time.deltaTime;
        var zIndex = pathToEnd[0].transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, pathToEnd[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);


        if (Vector2.Distance(transform.position, pathToEnd[0].transform.position) < 0.000001f)
        {
            PositionCharacter(pathToEnd[0]);
            pathToEnd.RemoveAt(0);
            isMoving = false;

        }
        
        if (pathToEnd.Count <= 0)
        {

            CalculateRange();
            isMoving = false;
            GameManager.Instance.endPlayerTurn();
            

        }
    }

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
                item.ShowTile();
            }
        }
    }

    public void FindPath(OverlayInfo overlayTile)
    {

        path = pathFinder.FindPath(activeTile, overlayTile, inRangeTiles);
        SpriteDirection(overlayTile);
    }
    public void FindEnd()
    {
        pathToEnd = pathFinder.FindPath(activeTile, GameManager.Instance.endTile, new List<OverlayInfo>());

       
        

        if (pathToEnd.Count > range - 1)
        {
            var toRemove = pathToEnd.Count - (range - 1);
            pathToEnd.RemoveRange(range - 1, toRemove);
            
        }
  
            

    }
    public void PositionCharacter(OverlayInfo tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        activeTile = tile;

        if (activeTile == GameManager.Instance.endTile && isPlayer)
        {
            GameManager.Instance.EndGame(this.gameObject, "Package Delivered!");
        }

        updateActiveTile(true);
    }


    private void SpriteDirection(OverlayInfo destination)
    {

        if (destination.transform.position.x > activeTile.transform.position.x)
            renderer.flipX = false;
        else if (destination.transform.position.x < activeTile.transform.position.x)
            renderer.flipX = true;
    }

    public void updateActiveTile(bool state)
    {
        activeTile.isBlocked = state;
    }
}
