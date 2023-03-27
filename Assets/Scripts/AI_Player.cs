using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Player : MovingCharacter
{
    private AI_Player playerChar;

    private void OnEnable()
    {
       playerChar = this;
        isPlayer = true;
 
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



}
