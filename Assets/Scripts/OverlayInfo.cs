using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OverlayInfo : MonoBehaviour
{
    public bool showTile;
    public bool hideTile;
    public bool isStart;
    public bool isEnd;

    public int gCost;
    public int hCost;

    public int fCost { get { return gCost + hCost; } }

    public bool isBlocked;
    public bool hasEnemy;
    public bool hasTrap;

    public OverlayInfo parent;
    public List<OverlayInfo> myNeighbours = new List<OverlayInfo>();

    public Tile tileType;

    public Vector3Int gridLocation;
    public Vector2Int gridLocation2D { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }


    public void ShowTile()
    {
       if(GridManager.Instance.debugging)
        {
            if (isBlocked)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(225, 0, 0, 0.3f);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 225, 0, 0.3f);
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
  
    }

    public void ShowEnemyTile()
    {
        if (GridManager.Instance.debugging)
        {
            if (isBlocked)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(225, 0, 0, 0.3f);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 225, 0.3f);
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0f);
    }



    public void setBlocked(bool status)
    {
        isBlocked = status;
    }

    public void setStart(bool status)
    {
        isStart = status;
    }

    public void setEnd(bool status)
    {
        isEnd = status;
    }





}
