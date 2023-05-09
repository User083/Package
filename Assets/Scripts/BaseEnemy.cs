using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseEnemy : MovingCharacter
{
    public BaseEnemy enemyScript;
    private AI_Player player;
    public int attackDamage;
    public bool awareOfPlayer;
    public bool moveComplete;
    public bool hasPackage;
    public GameObject trapPrefab;
    public GameObject packagePrefab;
    public enum State { Idle, Evaluate, Wander, Pursue, Attack }
    public State state;

    private void OnEnable()
    {
       enemyScript = this;
       state = State.Idle;

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
                if(path.Count() > 0)
                {
                    MoveTo();
                }
                else
                {
                    EndMyTurn();
                }
                
            }
            
        }
    }

    public void UpdateState()
    {
        switch(state)
        {

           case State.Evaluate:
                Evaluate();
                break; 
            case State.Attack:
                AttackPlayer();
                break;
            default:
                break;
        }
    }

    public void EndMyTurn()
    {
        moveComplete = true;
        state = State.Idle;
        GameManager.Instance.checkEnemiesState();
    }

    public void Evaluate()
    {
        moveComplete = false;
        if (CheckForPlayer())
        {
            //Chase or attack player
            OverlayInfo attackTile = GetRandomTileInRange(GameManager.Instance.gridManager.GetNeighbourTiles(player.activeTile, inRangeTiles));
            if(attackTile != null)
            {
                state = State.Pursue;
                FindPath(attackTile);
                speed = 4;
                UpdateState();
            }
            else
            {
                //Wander to random in range tile
                OverlayInfo randomTile = GetRandomTileInRange(inRangeTiles);
                if(randomTile != null)
                {
                    FindPath(randomTile);
                    state = State.Wander;
                    speed = 2;
                    UpdateState();
                }
                else
                {
                    EndMyTurn();
                }
                
            }          
        }
        else
        {
            OverlayInfo randomTile = GetRandomTileInRange(inRangeTiles);
            if (randomTile != null)
            {
                FindPath(randomTile);
                state = State.Wander;
                speed = 2;
                UpdateState();
            }
            else
            {
                EndMyTurn();
            }
            DropTrap();
            DropPackage();
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
            activeTile.activeEnemy = this;
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
                EndMyTurn();
            }
            CalculateRange();            
        }
       
    }

    public void FindPath(OverlayInfo overlayTile)
    {
        path = pathFinder.FindPath(activeTile, overlayTile, inRangeTiles);
        if(path.Contains(GameManager.Instance.endTile))
        {
            path.Remove(GameManager.Instance.endTile);
        }
        SpriteDirection(overlayTile);
    }
    //Damage Player on collision

    public void AttackPlayer()
    {
        GameManager.Instance.DamagePlayer(attackDamage);
        EndMyTurn();
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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Package")
        {
            hasPackage= true;
        }

    }

    private void DropTrap()
    {
        int chance = Random.Range(0, 100);
        if(chance <= GameManager.Instance.trapDropChance)
        {
            var tile = GetRandomTileInRange(inRangeTiles);
            if(tile != null)
            {
                var trap = Instantiate(trapPrefab);
                GameManager.Instance.PositionItem(tile, trap);
                GameManager.Instance.dropTrapList.Add(trap);
            }
            
        }

    }

    private void DropPackage()
    {
        if(hasPackage)
        {
            var tile = GetRandomTileInRange(inRangeTiles);
            if (tile != null)
            {
                var package = Instantiate(packagePrefab);
                GameManager.Instance.PositionItem(tile, package);
                hasPackage= false;
            }

        }
    }

}
