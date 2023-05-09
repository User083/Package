using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AI_Player : MovingCharacter
{
    private AI_Player playerChar;
    public int currentHealth;
    public bool isDead;
    public bool hasPackage;
    public Slider healthBar;
    private List<BaseEnemy> inRangeEnemies;
    
    public enum State {Idle, Evaluate, Seek, Evade, Retrieve, Heal}
    public State state;


    public List<OverlayInfo> pathToEnd = new List<OverlayInfo>();
    private void OnEnable()
    {
       playerChar = this;
        isPlayer = true;
        currentHealth = GameManager.Instance.agentMaxHealth;
        inRangeEnemies= new List<BaseEnemy>();
        hasPackage = true;
    }


    private void LateUpdate()
    {
        if(GameManager.Instance.turnState == GameManager.TurnState.PlayerTurn)
        {
            if(pathToEnd.Count() > 0)
            {
                MovePlayerTo();
            }
           
        } 

    }
    public void UpdateState()
    {
        switch (state)
        {
            case State.Idle:
                pathToEnd.Clear();
                break;
            case State.Evaluate:
                Evaluate();
                break;
            case State.Seek:
                FindEnd(GameManager.Instance.endTile);
                break;
            case State.Evade:
                Evade();
                break;
            case State.Retrieve:
                FindEnd(GameManager.Instance.packageTile);
                break;
            case State.Heal:
                FindEnd(FindHealth());
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
            EndMyTurn();
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
            if(tile.activeEnemy != null)
            {
                inRangeEnemies.Add(tile.activeEnemy);
                enemyList.Add(tile);
            }
        }
        return enemyList;
    }

    //State methods
        //Evaluate appropriate move
    public void Evaluate()
    {
        inRangeEnemies.Clear();
        var enemyCount = NearbyEnemies().Count();

        if (inRangeTiles.Contains(GameManager.Instance.endTile))
        {
            if (canRetrieve())
            {
                state = State.Retrieve;
            }
            else
            {
                state = State.Seek;
              
            }
            
        }
        else if (!inRangeTiles.Contains(GameManager.Instance.endTile))
        {
            if(canRetrieve())
            {
                state = State.Retrieve;
                
            }
            else if(canHeal())
            {
                state = State.Heal;
               
            }
            else if(enemyCount > 0)
            {
                state = State.Evade;     
            }
            else
            {
                state = State.Seek;

            }
            
        }
        UpdateState();
    }


        //End turn
    public void EndMyTurn()
    {
        state = State.Idle;
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

    public bool FindPackage(OverlayInfo packageTile)
    {
        if(inRangeTiles.Contains(packageTile))
        {
            return true;
        }
        else
        { return false; }
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / (float)GameManager.Instance.agentMaxHealth;
    }


    private bool canHeal()
    {
        if (GetHealthPercentage() <= GameManager.Instance.healthPercentage && FindHealth() != null && GameManager.Instance.GetTurnPercent() >= GameManager.Instance.turnPercentage)
        {
            return true;
        }

        return false;
    }

    private bool canRetrieve()
    {

        if (!hasPackage && inRangeTiles.Contains(GameManager.Instance.packageTile) && GameManager.Instance.GetTurnPercent() >= GameManager.Instance.turnPercentage)
        {
            return true;
        }
        return false;
    }

    private void Evade()
    {
        FindEnd(GameManager.Instance.endTile);

        foreach (var enemy in inRangeEnemies)
        {
            for (var i = 0; i < enemy.inRangeTiles.Count; i++)
            {
                if (pathToEnd.Contains(enemy.inRangeTiles[i]))
                {
                    pathToEnd.Remove(enemy.inRangeTiles[i]);
                    enemy.inRangeTiles[i].ShowEvasionTile();
                }
            }
        }

        if(pathToEnd.Count < 1)
        {
            FindEnd(GameManager.Instance.endTile);
        }
    }
}
