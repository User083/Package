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
        if (path.Count > 0 && playerTurn)
        {
            playerChar.MoveTo();
           
        }
    }

}
