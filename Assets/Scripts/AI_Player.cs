using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Player : MovingCharacter
{
    private AI_Player playerChar;
    public int maxHealth = 100;
    public int currentHealth;
    public List<OverlayInfo> pathToEnd = new List<OverlayInfo>();
    private void OnEnable()
    {
       playerChar = this;
        isPlayer = true;
        currentHealth = maxHealth;
    }


    private void LateUpdate()
    {
        if (pathToEnd.Count > 0 && playerTurn)
        {
            MovePlayerTo();

        }
        else if(playerTurn)
        {
            Debug.Log(pathToEnd.Count);
        }

   
    }
    private void Start()
    {
       
    }

    public void takeDamage(int damage)
    {
        if(currentHealth > damage)
        {
            currentHealth = currentHealth - damage;
        }
        else
        {
            GameManager.Instance.KillPlayer();
        }
    }

    public void MovePlayerTo()
    {
        var step = speed * Time.deltaTime;
        var zIndex = pathToEnd[0].transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, pathToEnd[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, pathToEnd[0].transform.position) < 0.000001f)
        {
            PositionCharacter(pathToEnd[0]);
            pathToEnd.RemoveAt(0);

        }
        if (pathToEnd.Count <= 0)
        {
            CalculateRange();
            GameManager.Instance.endPlayerTurn();
        }
    }

    //Find and calculate best path to the goal for the current turn
    public void FindEnd()
    {
        pathToEnd = pathFinder.FindPath(activeTile, GameManager.Instance.endTile, new List<OverlayInfo>());

        if (pathToEnd.Count > range - 1)
        {
            var toRemove = pathToEnd.Count - (range - 1);
            pathToEnd.RemoveRange(range - 1, toRemove);
        }
    }

}
