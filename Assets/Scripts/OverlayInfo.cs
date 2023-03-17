using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayInfo : MonoBehaviour
{
    public bool showTile;
    public bool debugging;

    public int gCost = 1;
    public int hCost;

    public int fCost { get { return gCost + hCost; } }

    public bool isBlocked;

    public OverlayInfo parent;

    public Vector3Int gridLocation;
    void Update()
    {
        if(showTile && debugging)
        {
            ShowTile();
        }
        else if(!showTile && debugging)
        {
            HideTile();
        }

        if(Input.GetMouseButtonDown(0)) 
        {
            HideTile();        
        }
    }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
