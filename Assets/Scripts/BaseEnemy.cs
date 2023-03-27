using System.Collections;
using System.Collections.Generic;
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
            speed = 4;
            FindPath(player.activeTile);
        }
        else
        {
            //Wander to random in range tile
            speed = 2;
            int i = Random.Range(0, inRangeTiles.Count - 1);
            FindPath(inRangeTiles[i]);
        }
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

}
