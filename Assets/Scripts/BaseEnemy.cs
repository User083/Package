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

    private void OnEnable()
    {
       enemyScript = this;
       
    }

    private void Start()
    {
        player = GameManager.Instance.playerChar;
    }

    private void LateUpdate()
    {
        if (path.Count > 0 && !playerTurn)
        {
            MoveTo();
        }
    }

    public void EnemyTurn()
    {
        if(CheckForPlayer())
        {
            //Chase or attack player
            Debug.Log("Enemy spotted player");
            isAttacking= true;
            speed = 4;
            range = 2;
            FindPath(player.activeTile);
        }
        else
        {
            //Wander to random in range tile
            isAttacking = false;
            speed = 2;
            range = 4;
            FindPath(GetWanderTile());
        }
 
    }

    //Damage Player on collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.takeDamage(attackDamage);
            Debug.Log("Player damaged");
        }
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


}
