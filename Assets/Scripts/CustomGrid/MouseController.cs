using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class MouseController : MonoBehaviour
{
    public GameObject AIPlayerPrefab;
    private AI_Player playerChar;
    private OverlayInfo overlayTile;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        playerChar = GameManager.Instance.playerChar;

    }

    private void Update()
    {
        
        //UpdateCursor();
     

    }

    public void UpdateCursor()
    {
        var focusedTileHit = GetFocusedOnTile();
        
        if (focusedTileHit.HasValue)
        {
            overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayInfo>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            //to be replaced with event system
            if (Input.GetMouseButtonDown(0))
            {

                if (playerChar == null)
                {

                    //SpawnChar(overlayTile);

                }
                else
                {

                    playerChar.FindPath(overlayTile);

                }
            }
        }
    }

    public void SpawnChar(OverlayInfo overlayTile)
    {
        playerChar = Instantiate(AIPlayerPrefab).GetComponent<AI_Player>();
        playerChar.PositionCharacter(overlayTile);
        playerChar.CalculateRange();
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


}
