using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("Singleton Manager")]
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public bool Debugging;

    [Header("Turn-Based Variables")]
    public int turnCount = 3;
    
    public enum TurnState { Processing, PlayerTurn, EnemyTurn, GameOver }
    public TurnState turnState;
    
    [Header("Game References")]
    public GameObject AIPlayerPrefab;
    public GameObject EnemyPrefab;
    public BaseEnemy enemy;
    public AI_Player playerChar;
    public OverlayInfo startTile;
    public OverlayInfo endTile;

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
                Delay(1f);
                ProcessTurns();
                Debug.Log(turnState);
                break;
            case TurnState.PlayerTurn:
                PlayerTurn();
                Debug.Log(turnState);
                break; 
            case TurnState.EnemyTurn:
                EnemyTurn(enemy);
                Debug.Log(turnState);
                break;
            case TurnState.GameOver:
                Debug.Break();
                break;
            default:
                break;
        }
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
            EndGame("Run out of turns - Mission Failure");
            return;
        }

        turnCount--;
        turnState = TurnState.PlayerTurn;
        UpdateState();
    }
    public void EnemyTurn(BaseEnemy self)
    {
        Debug.Log("Enemy turn starts");
        self.state = BaseEnemy.State.Evaluate;
        self.UpdateState();
     
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
        turnState = TurnState.GameOver;
        UpdateState();
    }

    //Stop the turn cycle of movement to give AI chance to choose their action
    public void TriggerCombat()
    {

    }

    //Utility methods
    public IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
