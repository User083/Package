using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEnemy : MovingCharacter
{
    public BaseEnemy enemyScript;
    private AI_Player player;
    public int attackDamage = 20;
    public bool awareOfPlayer;
    public enum State { Wait, Evaluate, Wander, Pursue, Attack, EndTurn }
    public State state;

    private void OnEnable()
    {
       enemyScript = this;
       state = State.Wait;

    }

    private void Start()
    {
        player = GameManager.Instance.playerChar;
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.turnState == GameManager.TurnState.EnemyTurn)
        {
            if(state == State.Wander || state == State.Pursue)
            {
                MoveTo();
            }
            
        }
    }

    public void UpdateState()
    {
        switch(state)
        {
           case State.Wait:
                Debug.Log("Enemy state: " + state);
                break;
           case State.Evaluate:
                Evaluate();
                Debug.Log("Enemy state: " + state);
                break; 
           case State.Wander:
                Debug.Log("Enemy state: " + state);
                break;
            case State.Attack:
                AttackPlayer();
                Debug.Log("Enemy state: " + state);
                break;
            case State.EndTurn:
                EndMyTurn();
                Debug.Log("Enemy state: " + state);
                break;
            default:
                break;
        }
    }

    public void EndMyTurn()
    {
        state = State.Wait;
        UpdateState();
        GameManager.Instance.turnState = GameManager.TurnState.Processing;
        GameManager.Instance.UpdateState();
    }

    public void Evaluate()
    {
        if(CheckForPlayer())
        {
            //Chase or attack player
            Debug.Log("Enemy spotted player");
            OverlayInfo attackTile = GetAttackTile(GridManager.Instance.GetNeighbourTiles(player.activeTile, inRangeTiles));
            Debug.Log(attackTile);
            if(attackTile != null)
            {
                state = State.Pursue;
                FindPath(attackTile);
                speed = 4;
                range = 2;
                GameManager.Instance.Delay(1f);
                UpdateState();
            }
            else
            {
                //Wander to random in range tile
                FindPath(GetWanderTile());
                state = State.Wander;
                speed = 2;
                range = 4;
                GameManager.Instance.Delay(1f);
                UpdateState();
            }
            
        }
        else
        {
            //Wander to random in range tile
            FindPath(GetWanderTile());           
            state = State.Wander;
            speed = 2;
            range = 4;
            GameManager.Instance.Delay(1f);
            UpdateState();
        }
        
    }


    public void MoveTo()
    {
        updateActiveTile(false);

        var step = speed * Time.deltaTime;
        var zIndex = path[0].transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);


        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.000001f)
        {
            PositionCharacter(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            

            if(state == State.Pursue)
            {
                state = State.Attack;
                UpdateState();
            }
            else
            {
                state = State.EndTurn;
                UpdateState();
            }
            CalculateRange();
            

        }

    }

    public void FindPath(OverlayInfo overlayTile)
    {
        path = pathFinder.FindPath(activeTile, overlayTile, inRangeTiles);
        SpriteDirection(overlayTile);
    }
    //Damage Player on collision

    public void AttackPlayer()
    {
        player.takeDamage(attackDamage);
        state = State.EndTurn;
        UpdateState();
    }
    private bool CheckForPlayer()
    {
        if(inRangeTiles.Contains(player.activeTile))
        {              
            return true;
        }
        else
        {
            return false;
        }
  
    }

    private OverlayInfo GetWanderTile()
    {
        List<OverlayInfo> tempList = new List<OverlayInfo>();

        foreach (OverlayInfo tile in inRangeTiles)
        {
            if (tile.isBlocked == false)
            {
                tempList.Add(tile);
            }
        }
        int i = Random.Range(0, tempList.Count);

        return tempList.ElementAt(i);
    }

    private OverlayInfo GetAttackTile(List<OverlayInfo> list)
    {
        List<OverlayInfo> tempList = new List<OverlayInfo>();

        foreach (OverlayInfo tile in list)
        {
            if (!tile.isBlocked)
            {
                tempList.Add(tile);
            }
        }
        int i = Random.Range(0, tempList.Count);

        return tempList.ElementAt(i);
    }

}
