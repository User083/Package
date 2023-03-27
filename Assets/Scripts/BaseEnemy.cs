using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MovingCharacter
{
    private BaseEnemy enemyScript;
    private AI_Player player;
 


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
        if (path.Count > 0)
        {
            MoveTo();
        }
        else
        {
            Debug.Log("Enemy can't find path");
        }
    }

    public void EnemyTurn()
    {
        int i = Random.Range(0, inRangeTiles.Count);
        FindPath(inRangeTiles[i]);
      
    }
    

}
