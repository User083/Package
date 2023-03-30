using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("Singleton Manager")]
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public bool Debugging;
    public UIManager UIManager;

    [Header("Turn-Based Variables")]
    public int turnCount = 3;
    
    public enum TurnState { Processing, PlayerTurn, EnemyTurn, GameOver }
    public TurnState turnState;
    
    [Header("Game References")]
    public GameObject AIPlayerPrefab;
    public GameObject EnemyPrefab;
    public BaseEnemy enemy;
    public List<BaseEnemy> enemyList = new List<BaseEnemy>();
    public AI_Player playerChar;
    public OverlayInfo startTile;
    public OverlayInfo endTile;
    public Sprite deadPlayer;
    public Sprite openDoor;

    [Header("Conditions")]
    public bool endTileReached;
       



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


    private void Start()
    {
        
        SpawnCharacter(startTile);
        SpawnEnemy(GridManager.Instance.GetRandomTile());
        SpawnEnemy(GridManager.Instance.GetRandomTile());
        Delay(2f);
        turnState = TurnState.PlayerTurn;
        UpdateState();
    }

    private void LateUpdate()
    {
        
        
    }

    public void UpdateState()
    {
        switch (turnState)
        {
            case TurnState.Processing:
                ProcessTurns();
                Debug.Log(turnState);
                break;
            case TurnState.PlayerTurn:
                PlayerTurn();
                Debug.Log(turnState);
                break; 
            case TurnState.EnemyTurn:
                EnemyTurn();
                Debug.Log(turnState);
                break;
            case TurnState.GameOver:
                Debug.Break();
                break;
            default:
                break;
        }
        UIManager.UpdateUI(turnCount.ToString(), turnState.ToString(), playerChar.state.ToString(), enemy.state.ToString(), playerChar.currentHealth.ToString());
    }

    private void PlayerTurn()
    {
        Debug.Log("Player turn starts. " + turnCount + " turns left.");
        playerChar.state = AI_Player.State.Evaluate;
        playerChar.UpdateState();
       
      
    }

    public void ProcessTurns()
    {
        if (endTileReached)
        {
            EndGame("Package Delivered - Mission Success");
            return;
        }

        if (playerChar.isDead)
        {
            EndGame("Agent died - Mission Failure");
            return;
        }

        if (turnCount <=0)
        {
            EndGame("Ran out of turns - Mission Failure");
            return;
        }

        turnCount--;
        turnState = TurnState.PlayerTurn;
        UpdateState();
    }
    public void EnemyTurn()
    {
        if(enemyList.Count > 0)
        {
            foreach(var enemy in enemyList)
            {
                enemy.state = BaseEnemy.State.Evaluate;
                enemy.UpdateState();
            }
        }
        else
        {
            Debug.Log("No Enemies left");
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
        enemyList.Add(enemy);
        enemy.PositionCharacter(start);
        enemy.CalculateRange();
    }
    public void EndGame(string condition)
    {
        Debug.Log("Game over! " + condition );
        UIManager.UpdateUI(turnCount.ToString(), turnState.ToString(), playerChar.state.ToString(), enemy.state.ToString(), playerChar.currentHealth.ToString());
        UIManager.UpdateGameOver(condition);
        //playerChar.gameObject.SetActive(false);
        turnState = TurnState.GameOver;
        UpdateState();
        
    }

    //Utility methods
    public IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }

    public void checkEnemiesState()
    {
        foreach(var enemy in enemyList)
        {
            if(!enemy.moveComplete)
            {
                return;
            }
        }
        
        turnState = TurnState.Processing;
        UpdateState();
    }

    public void DamagePlayer(int damage)
    {

        if (playerChar.currentHealth > damage)
        {
            playerChar.currentHealth = playerChar.currentHealth - damage;
        }
        else
        {
            playerChar.currentHealth = 0;
            playerChar.isDead = true;
            playerChar.spriteRenderer.sprite = deadPlayer;
        }

    }
}
