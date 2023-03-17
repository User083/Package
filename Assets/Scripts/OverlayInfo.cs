using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayInfo : MonoBehaviour
{
    public bool showTile;
    public bool hideTile;
    public bool debugging;

    public int gCost;
    public int hCost;

    public int fCost { get { return gCost + hCost; } }

    public bool isBlocked;

    public OverlayInfo parent;

    public Vector3Int gridLocation;
    public Vector2Int gridLocation2D { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

  

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0f);
    }

    
}
