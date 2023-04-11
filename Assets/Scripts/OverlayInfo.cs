using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    public bool hasPackage;
    public int trapDamage;
    public int healAmount;
    private GameObject tileObject = null;

    

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
        healAmount = GameManager.Instance.potionHeal;
    }
    public void ShowTile()
    {
       if(GameManager.Instance.Debugging)
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

  
    }

    public void ShowEnemyTile()
    {
        if (GameManager.Instance.Debugging)
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

        if (hasHealth && gCost >= 1 && GameManager.Instance.playerChar.currentHealth < 50)
        {
            gCost = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HealthPotion" && !hasHealth)
        {
            hasHealth = true;
            tileObject = collision.gameObject;
        }

        if (collision.gameObject.tag == "Package" && !hasPackage)
        {
            hasPackage = true;
            tileObject = collision.gameObject;
            isBlocked = false;
        }

        if (collision.gameObject.tag == "Player")
        {
            if(hasHealth)
            {
                GameManager.Instance.HealPlayer(healAmount);                
                Destroy(tileObject);
                hasHealth = false;
            }

            if(hasTrap)
            {
                GameManager.Instance.DamagePlayer(trapDamage);
            }

            if(hasPackage)
            {
                GameManager.Instance.packageTile = null;
                Destroy(tileObject);
                hasPackage = false;
            }
            
        }

        if(collision.gameObject.tag == "Enemy")
        {
            if(hasPackage)
            {
                Destroy(tileObject);
                hasPackage = false;
                GameManager.Instance.packageTile = null;
            }
        }
    }

}
