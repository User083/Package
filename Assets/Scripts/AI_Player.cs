using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Player : MovingCharacter
{
    private AI_Player playerChar;
    public int currentHealth;
    public bool isDead;
    public bool hasPackage;
    
    public enum State {Wait, Evaluate, Seek, Flee, Combat, EndTurn }
    public State state;


    public List<OverlayInfo> pathToEnd = new List<OverlayInfo>();
    private void OnEnable()
    {
       playerChar = this;
        isPlayer = true;
        currentHealth = GameManager.Instance.agentMaxHealth;
        state = State.Wait;
        hasPackage = true;
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
                break;
            case State.Evaluate:
                Evaluate();
                break;
            case State.Seek:
                break;
            case State.Flee:
                break;
            case State.Combat:
                break;
            case State.EndTurn:
                EndMyTurn();
                break;
            default:
                break;
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
        if (pathToEnd.Count() <= 0)
        {
            state = State.EndTurn;
            UpdateState();
            CalculateRange();
        }
    }

    //Find and calculate best path to the goal for the current turn
    public void FindEnd(OverlayInfo destination)
    {
        pathToEnd = pathFinder.FindPath(activeTile, destination, new List<OverlayInfo>());

        if (pathToEnd.Count() > range - 1)
        {
            var toRemove = pathToEnd.Count() - (range - 1);
            pathToEnd.RemoveRange(range - 1, toRemove);
            
        }
        if(pathToEnd.Count() < 0)
        {
            SpriteDirection(pathToEnd[pathToEnd.Count() - 1]);
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
        var enemyCount = NearbyEnemies().Count();
        

        //if(enemyCount > 0)
        //{
        //    Debug.Log("Nearby enemies: " + enemyCount);
        //    FindEnd(GameManager.Instance.endTile);
        //    GameManager.Instance.Delay(1f);
        //    CheckPath();
        //}
        if(GetHealthPercentage() < 50 && FindHealth() != null)
        {
                FindEnd(FindHealth());
                CheckPath();      
        }
        else
        {
            FindEnd(GameManager.Instance.endTile);
            GameManager.Instance.Delay(1f);
            CheckPath();
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

    public OverlayInfo FindHealth()
    {
        foreach(var tile in inRangeTiles)
        {
            if(tile.hasHealth)
            {
                return tile;
            }
        }

        return null;
    }

    public float GetHealthPercentage()
    {
        return Mathf.RoundToInt(currentHealth / GameManager.Instance.agentMaxHealth * 100);
    }

    public void CheckPath()
    {
        if (pathToEnd.Count() > 0)
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
