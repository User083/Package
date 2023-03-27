using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public bool Debugging;

    public GameObject AIPlayerPrefab;
    public GameObject EnemyPrefab;
    public BaseEnemy enemy;
    public AI_Player playerChar;
    public OverlayInfo startTile;
    public OverlayInfo endTile;

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

       
    }
    private void OnEnable()
    {
        
    }

    private void Start()
    {
        SpawnCharacter(startTile);
        SpawnEnemy(GridManager.Instance.GetRandomTile());
        startPlayerTurn();
    }

    public void endPlayerTurn()
    {
        if(turnCount > 0)
        {
            turnCount--;
            playerChar.playerTurn = false;
            startEnemyTurn(enemy); 
        }
        else
        {
            EndGame("Ran out of turns!");
        }
        Debug.Log("Player turn ends");

    }

    private void startPlayerTurn()
    {
        Debug.Log("Player turn starts. " + turnCount + " turns left.");
        playerChar.playerTurn = true;
        playerChar.FindEnd();
      
    }

    public void startEnemyTurn(BaseEnemy self)
    {
        Debug.Log("Enemy turn starts");
        self.EnemyTurn();
     
    }

    public void endEnemyTurn()
    {
        Debug.Log("Enemy turn ends");
        if (playerChar.isActiveAndEnabled)
        {
            startPlayerTurn();
        }
        else
        {
            EndGame("Agent is dead");
        }
        
    }

    public void SpawnCharacter(OverlayInfo start)
    {
        playerChar = Instantiate(AIPlayerPrefab).GetComponent<AI_Player>();
        playerChar.PositionCharacter(start);
        playerChar.CalculateRange();
    }

    public void SpawnEnemy(OverlayInfo start)
    {
        enemy = Instantiate(EnemyPrefab).GetComponent<BaseEnemy>();
        enemy.PositionCharacter(start);
        enemy.CalculateRange();
    }
    public void EndGame(string condition)
    {
        Debug.Log("Game over! " + condition );
        playerChar.gameObject.SetActive(false);
    }

    public void KillPlayer()
    {
        Debug.Log("Agent died");
        playerChar.gameObject.SetActive(false);

    }

    
}
