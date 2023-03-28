using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Player : MovingCharacter
{
    private AI_Player playerChar;
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead;
    public enum State {Wait, Evaluate, Seek, Flee, Combat, EndTurn }
    public State state;


    public List<OverlayInfo> pathToEnd = new List<OverlayInfo>();
    private void OnEnable()
    {
       playerChar = this;
        isPlayer = true;
        currentHealth = maxHealth;
        state = State.Wait;
    }


    private void LateUpdate()
    {
        if(GameManager.Instance.turnState == GameManager.TurnState.PlayerTurn)
        {
            if(state == State.Seek)
            {
                MovePlayerTo();
            }
           
        }

        

    }
    public void UpdateState()
    {
        switch (state)
        {
            case State.Wait:
                Debug.Log("Player state: " + state);
                break;
            case State.Evaluate:
                Debug.Log("Player state: " + state);
                Evaluate();
                break;
            case State.Seek:
                Debug.Log("Player state: " + state);
                break;
            case State.Flee:
                Debug.Log("Player state: " + state);
                break;
            case State.Combat:
                Debug.Log("Player state: " + state);
                break;
            case State.EndTurn:
                EndMyTurn();
                Debug.Log("Player state: " + state);
                break;
            default:
                break;
        }
    }

    public void takeDamage(int damage)
    {
        
        if(currentHealth > damage)
        {
            currentHealth = currentHealth - damage;
        }
        else
        {
            isDead = true;
        }
        
    }

    //Might turn this into a while loop
    public void MovePlayerTo()
    {
        updateActiveTile(false);
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
            state = State.EndTurn;
            UpdateState();
            CalculateRange();
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

    //Find any enemies within range
    private List<OverlayInfo> NearbyEnemies()
    {
        List<OverlayInfo> enemyList = new List<OverlayInfo>();
        foreach(var tile in inRangeTiles)
        {
            if(tile.hasEnemy)
            {
                enemyList.Add(tile);
            }
        }
        return enemyList;
    }

    //State methods
        //Evaluate appropriate move
    public void Evaluate()
    {
        var enemyCount = NearbyEnemies().Count;

        if(enemyCount > 0)
        {
            Debug.Log("Nearby enemies: " + enemyCount);
            FindEnd();
            GameManager.Instance.Delay(1f);
            if (pathToEnd.Count > 0)
            {
                state = State.Seek;
                UpdateState();
            }
            else
            {
                state = State.EndTurn;
                UpdateState();
            }
        }
        else
        {
            FindEnd();
            GameManager.Instance.Delay(1f);
            if (pathToEnd.Count > 0)
            {
                state = State.Seek;
                UpdateState();
            }
            else
            {
                state = State.EndTurn;
                UpdateState();
            }


        }
    }

        //End turn
    public void EndMyTurn()
    {
        state = State.Wait;
        GameManager.Instance.turnState = GameManager.TurnState.EnemyTurn;
        UpdateState();
        GameManager.Instance.UpdateState();
    }


}
