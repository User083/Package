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

}
