using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MouseController : MonoBehaviour
{
    public GameObject AIPlayerPrefab;
    private AI_Player playerChar;

    private PathfindingCore pathFinder;
    private List<OverlayInfo> path = new List<OverlayInfo>();

    private void Start()
    {
        playerChar= AIPlayerPrefab.GetComponent<AI_Player>();
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
                overlayTile.GetComponent<OverlayInfo>().ShowTile();

                if (playerChar == null)
                {
                    Debug.LogWarning("no character found in scene");
                    
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
        var step = playerChar.speed * Time.deltaTime;

        var zIndex = path[0].transform.position.z;

        playerChar.curPosition = Vector2.MoveTowards(playerChar.transform.position, path[0].transform.position, step);
        playerChar.curPosition = new Vector3(playerChar.curPosition.x, playerChar.curPosition.y, zIndex);

        if(Vector2.Distance(playerChar.curPosition, path[0].transform.position) <0.0001f)
        {
            PositionCharacter(path[0]);
            path.RemoveAt(0);
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
        playerChar.curPosition = new Vector3(tile.transform.position.x, tile.transform.position.y+000.1f, tile.transform.position.z);
        playerChar.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        //playerChar.activeTile = tile;
    }
}
