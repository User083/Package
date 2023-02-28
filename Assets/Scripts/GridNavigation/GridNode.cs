using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    Grid m_Generator;

    [SerializeField]
    UnityEngine.Color m_WalkableColour;
    [SerializeField]
    UnityEngine.Color m_NotWalkableColour;

    [SerializeField]
    UnityEngine.Color m_ClosedInPathFindingColour;

    [SerializeField]
    UnityEngine.Color m_OpenInPathFindingColour;

    [SerializeField]
    UnityEngine.Color m_PathInPathFindingColour;

    public bool m_Walkable;
    public float m_Cost = 1;
    public bool m_visited = false;
    public Vector2 position;

    public float gCost;
    public float hCost;
    public float fCost;
    public GridNode tileParent;
    public GridNode self;
    public bool isObstacleTile;


    /// <summary>
    /// Neighbouring nodes on the grid starting with up and going clockwise
    /// 0 - up
    /// 1 - up right
    /// 2 - right
    /// 3 - down right
    /// 4 - down
    /// 5 - down left
    /// 6 - left
    /// 7 - up left
    /// null if no neighbours
    /// </summary>
    GridNode[] m_Neighbours;
    public GridNode[] Neighbours { get { return m_Neighbours; } private set { } }

    public void Init(Grid generator, GridNode up, GridNode upRight, GridNode right, GridNode downRight, GridNode down, GridNode downLeft, GridNode left, GridNode upLeft)
    {
        m_Neighbours = new GridNode[8] { up, upRight, right, downRight, down, downLeft, left, upLeft };
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Generator = generator;
        position = (Vector2)transform.position;
        self = this;
        tileType(2);
        UpdateWalkable();
        updateTileCosts(null, m_Cost, 0f );
        
    }

    public void tileType(float difficultyScore)
    {
        if(isObstacleTile)
        {
            m_Cost = difficultyScore;
        }
    }


     public void updateTileCosts(GridNode parent, float gCost, float hCost)
    {

        this.hCost = hCost;
        fCost = gCost + hCost;
        tileParent = parent;

        if (tileParent != null)
        {
            this.gCost = parent.gCost + gCost;
        }
        else
        {
            this.gCost = gCost; 
        }
      
    }

    
    public void UpdateWalkable()
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        Physics2D.BoxCast(transform.position, transform.localScale, transform.rotation.eulerAngles.z, transform.forward, m_Generator.m_ContactFilter, hits);

        if(hits[0])
        {
            m_Walkable = false;
            m_visited = true;
            m_Cost = float.PositiveInfinity;
        }
        else
        {
            m_Walkable = true;
        }

        SetWalkableColour();
    }

    public void SetWalkableColour()
    {
        if (m_Walkable)
        {
            m_SpriteRenderer.color = m_WalkableColour;
        }
        else
        {
            m_SpriteRenderer.color = m_NotWalkableColour;
        }
    }

    public void ShowGrid()
    {
        m_SpriteRenderer.enabled = !m_SpriteRenderer.enabled;
    }

    public void ShowGrid(bool show)
    {
        m_SpriteRenderer.enabled = show;
    }

    public void SetClosedInPathFinding()
    {
        m_SpriteRenderer.color = m_ClosedInPathFindingColour;
    }

    public void SetOpenInPathFinding()
    {
        m_SpriteRenderer.color = m_OpenInPathFindingColour;
    }

    public void SetPathInPathFinding()
    {
        m_SpriteRenderer.color = m_PathInPathFindingColour;
    }
}
