using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MouseController : MonoBehaviour
{
    public GameObject AIPlayerPrefab;
    private AI_Player playerChar;
    private RangeFinder rangeFinder;

    private PathfindingCore pathFinder;
    private List<OverlayInfo> path = new List<OverlayInfo>();
    private List<OverlayInfo> inRangeTiles = new List<OverlayInfo>();

    private void Start()
    {
        pathFinder = new PathfindingCore();
        rangeFinder= new RangeFinder();
    }
    private void LateUpdate()
    {
        var focusedTileHit = GetFocusedOnTile();
        if(focusedTileHit.HasValue)
        {
            OverlayInfo overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayInfo>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            //to be replaced with event system
            if(Input.GetMouseButtonDown(0))
            {
               

                if (playerChar == null)
                {
                    Debug.LogWarning("no character found in scene");

                    
                    playerChar = Instantiate(AIPlayerPrefab).GetComponent<AI_Player>();
                    PositionCharacter(overlayTile);
                    GetInRangeTiles();
                    
                    
                }
                else
                {
                   
                    path = pathFinder.FindPath(playerChar.activeTile, overlayTile, inRangeTiles);
                    

                }
            }
        }

        //Move whenever there is a path
        if (path.Count > 0)
        {
            MoveOnPath();
        }

    }

private void GetInRangeTiles()
    {
        foreach (var item in inRangeTiles)
        {
            item.HideTile();
        }

        inRangeTiles = rangeFinder.GetTilesInRange(playerChar.activeTile, 3);

        foreach (var item in inRangeTiles)
        {
            item.ShowTile();
        }

       
    }

    private void MoveOnPath()
    {
        var step = 4f * Time.deltaTime;

        var zIndex = path[0].transform.position.z;

        playerChar.transform.position = Vector2.MoveTowards(playerChar.transform.position, path[0].transform.position, step);
        playerChar.transform.position = new Vector3(playerChar.transform.position.x, playerChar.transform.position.y, zIndex);

        if(Vector2.Distance(playerChar.transform.position, path[0].transform.position) < 0.000001f)
        {
            PositionCharacter(path[0]);
            path.RemoveAt(0);
        }
        
        if(path.Count == 0)
        {
         
            GetInRangeTiles();
        }
    }

    public RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
        if(hits.Length > 0)
        {
            return hits.OrderByDescending(i=>i.collider.transform.position.z).First();

        }

        return null;
    }

    private void PositionCharacter(OverlayInfo tile)
    {   
        playerChar.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y+0.0001f, tile.transform.position.z);
        playerChar.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        playerChar.activeTile = tile;
    }
}
