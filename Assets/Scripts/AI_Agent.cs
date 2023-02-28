using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent : MonoBehaviour
{
    protected Animator m_Animator;
    protected SpriteRenderer m_Renderer;
    Rigidbody2D m_Rigidbody;
    AStar pathfinding;
    public Vector2 destination;
    public float speed;


    protected virtual void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        pathfinding = new AStar();

    }
    protected virtual void FixedUpdate()
    {
        Movement();
    }
    protected void Movement()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        if (m_Rigidbody.velocity.x <= 0.1f)
            m_Renderer.flipX = false;
        else if (m_Rigidbody.velocity.x >= 0.1f)
            m_Renderer.flipX = true;
    }

    protected void Update()
    {
        if (pathfinding.m_Path.Count == 0)
        {
            Rect size = Grid.m_GridSize;
            float x1 = Random.Range(size.xMin, size.xMax);
            float y1 = Random.Range(size.yMin, size.yMax);

            pathfinding.GeneratePath(Grid.GetNodeClosestWalkableToLocation(transform.position), Grid.GetNodeClosestWalkableToLocation(new Vector2(x1, y1)));
        }
        else
        {
            if (pathfinding.m_Path.Count > 0)
            {
                Vector2 closestPoint = pathfinding.GetClosestPointOnPath(transform.position);

                if (Maths.Magnitude(closestPoint - (Vector2)transform.position) < 0.5f)
                    closestPoint = pathfinding.GetNextPointOnPath(transform.position);

                destination = closestPoint;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawLine(transform.position, destination);

            if (pathfinding.m_Path.Count > 1)
            {
                for (int i = 0; i < pathfinding.m_Path.Count - 1; ++i)
                {
                    Gizmos.DrawLine(pathfinding.m_Path[i], pathfinding.m_Path[i + 1]);
                }
            }
        }
    }

}
