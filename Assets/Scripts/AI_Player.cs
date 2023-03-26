using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Player : MonoBehaviour
{
    private AI_Player playerChar;
    public RangeFinder rangeFinder;
    private PathfindingCore pathFinder;
    public List<OverlayInfo> path = new List<OverlayInfo>();
    public List<OverlayInfo> inRangeTiles = new List<OverlayInfo>();
    public List<OverlayInfo> pathToEnd = new List<OverlayInfo>();
    public OverlayInfo activeTile = null;
    public float speed = 4f;
    public int range = 3;
    public GameObject player;
  
    private bool moveCompleted;

    private void OnEnable()
    {
        pathFinder = new PathfindingCore();
        rangeFinder = new RangeFinder();
        playerChar = this;
    }
    private void Start()
    {
        FindEnd();
        Debug.Log(pathToEnd.Count);
    }

    private void LateUpdate()
    {
        if (path.Count > 0)
        {
            playerChar.MoveTo();
        }
    }



    public void MoveTo()
    {
        var step = speed * Time.deltaTime;

        var zIndex = path[0].transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.000001f)
        {
            PositionCharacter(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {

            CalculateRange();
            moveCompleted = true;
            GameManager.Instance.endPlayerTurn();
        }
    }

    public void CalculateRange()
    {
        if(inRangeTiles.Count > 0)
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
    }
    public void FindEnd()
    {
        pathToEnd = pathFinder.FindPath(activeTile, GameManager.Instance.endTile, new List<OverlayInfo>());
    }
    public void PositionCharacter(OverlayInfo tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        activeTile = tile;

        if(activeTile == GameManager.Instance.endTile)
        {
            GameManager.Instance.Win(this.gameObject);
        }
    }




}
