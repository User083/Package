using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class OverlayInfo : MonoBehaviour
{

    [Header ("status")]
    public bool showTile;
    public bool hideTile;
    public bool isStart;
    public bool isEnd;
    public bool isBlocked;
    public bool hasEnemy;
    public bool hasTrap;
    public bool isTree;
    public bool isRoad;
    public bool hasHealth;
    public int trapDamage;

    

    [Header("pathfinding")]
    public int gCost;
    public int hCost;
    public int fCost { get { return gCost + hCost; } }
    public OverlayInfo parent;
    public List<OverlayInfo> myNeighbours = new List<OverlayInfo>();

    public Tile tileType;

    public Vector3Int gridLocation;
    public Vector2Int gridLocation2D { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

    private void Start()
    {
        trapDamage = GameManager.Instance.trapDamage;
    }
    public void ShowTile()
    {
       if(GridManager.Instance.debugging)
        {
            if (isBlocked)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(225, 0, 0, 0.3f);
            }
            else if (isTree)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(225, 225, 0, 0.5f);
            }
            else if (hasTrap)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new Color(165, 165, 0, 0.5f);
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


    public void setStatus()
    {
        if(isTree)
        {
            gCost = gCost + 1;
        }

        if(hasTrap && GameManager.Instance.playerChar.currentHealth < 50)
        {
            gCost = gCost + 3;
        }
        else
        {
            gCost = gCost + 1;
        }

        if(isRoad && gCost >= 1)
        {
            gCost = gCost - 1;
        }

        if(hasEnemy)
        {
            gCost = gCost + 2;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && hasTrap)
        {
            GameManager.Instance.DamagePlayer(trapDamage);
        }
    }


}
