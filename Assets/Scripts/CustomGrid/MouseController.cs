using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MouseController : MonoBehaviour
{
    public GameObject AIPlayerPrefab;
    private AI_Player playerChar;
    public bool test = true;

    private PathfindingCore pathFinder;
    private List<OverlayInfo> path = new List<OverlayInfo>();

    private void Start()
    {
        pathFinder = new PathfindingCore();
    }
    private void Update()
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
                overlayTile.GetComponent<OverlayInfo>().ShowTile();

                if (test == true)
                {
                    Debug.LogWarning("no character found in scene");

                    
                    playerChar = Instantiate(AIPlayerPrefab).GetComponent<AI_Player>();
                    PositionCharacter(overlayTile);
                    Debug.Log(playerChar);
                    test = false;
                    
                }
                else
                {
                    Debug.Log(playerChar.activeTile);
                    
                    path = pathFinder.FindPath(playerChar.activeTile, overlayTile);
                }
            }
        }

        //Move whenever there is a path
        if(path.Count > 0)
        {
            MoveOnPath();
        }
        
    }

    private void MoveOnPath()
    {
        var step = 4f * Time.deltaTime;

        var zIndex = path[0].transform.position.z;

        playerChar.transform.position = Vector2.MoveTowards(playerChar.transform.position, path[0].transform.position, step);
        playerChar.transform.position = new Vector3(playerChar.transform.position.x, playerChar.transform.position.y, zIndex);

        if(Vector2.Distance(playerChar.curPosition, path[0].transform.position) <0.0001f)
        {
            PositionCharacter(path[0]);
            path.RemoveAt(0);
        }
        else
        {
            Debug.Log(path[0]);
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
    {   playerChar.activeTile = tile;
        playerChar.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y+0.0001f, tile.transform.position.z);
        playerChar.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        
    }
}
