using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public bool Debugging;

    public GameObject player;
    private AI_Player playerChar;

    public int turnCount = 3;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        if(player!= null)
        {
            playerChar= player.GetComponent<AI_Player>();
        }

        
    }

    public void endPlayerTurn()
    {
        turnCount--;
        startPlayerTurn();
    }

    private void startPlayerTurn()
    {
        Debug.Log(turnCount);
    }




}
