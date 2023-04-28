using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header ("Singleton Manager")]
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public bool Debugging;
    [SerializeField] private HUDManager HUDManager;

    [Header("Turn-Based Variables")]
    public int maxTurnCount = 7;
    private int turnCount;
    public int maxLifeCount = 3;
    private int lifeCount;
    private int score;

    [Header("Central Control")]
    public int trapDamage = 20;
    public int agentMaxHealth = 100;
    public int enemyDamage = 20;
    public int potionHeal = 20;
    public int deathScore = -20;
    public int turnScore = -10;
    public int deliveredScore = 50;
    public int survivedScore = 20;
    public int packageRecoveryScore = 20;
    public int enemiesToSpawn = 2;
    public int potionsToSpawn = 3;
    public int trapDropChance = 5;
    public enum TurnState { Processing, PlayerTurn, EnemyTurn, GameOver }
    public TurnState turnState;

    [Header("Prefabs")]
    public GameObject packagePrefab;
    public GameObject AIPlayerPrefab;
    public GameObject EnemyPrefab;
    public GameObject PotionPrefab;

    [Header("Game References")]
    public BaseEnemy enemy;
    public List<BaseEnemy> enemyList = new List<BaseEnemy>();
    public List<GameObject> potionList = new List<GameObject>();
    public List<GameObject> dropTrapList = new List<GameObject>();
    public AI_Player playerChar;
    public OverlayInfo startTile;
    public OverlayInfo endTile;
    public OverlayInfo packageTile = null;
    public Sprite deadPlayer;
    public Sprite livingPlayer;

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

        turnCount = maxTurnCount;
        lifeCount = maxLifeCount;
    }


    private void Start()
    {

        SpawnCharacter(startTile);

    }

    public void StartSimulation()
    {
        ApplySettings();        
        SpawnEnemies();
        SpawnPotions();
        HUDManager.UpdateUI(turnCount.ToString(), score.ToString(), lifeCount.ToString(), turnState.ToString(), PackageState());
        Delay(2f);
        turnState = TurnState.PlayerTurn;
        UpdateState();
    }

    private void ApplySettings()
    {
        score = 0;
        maxTurnCount = HUDManager.turns.value;
        agentMaxHealth = HUDManager.agentHealth.value;
        playerChar.healthBar.maxValue = agentMaxHealth;
        playerChar.currentHealth= agentMaxHealth;
        playerChar.healthBar.value = playerChar.currentHealth;
        trapDamage = HUDManager.trapDamage.value;
        potionHeal = HUDManager.healing.value;
        maxLifeCount = HUDManager.lives.value;
        playerChar.range = HUDManager.agentRange.value;
        Debugging = HUDManager.debug.value;
        enemiesToSpawn= HUDManager.enemySlider.value;
        turnCount = maxTurnCount;
        lifeCount = maxLifeCount;
        potionsToSpawn = HUDManager.potions.value;
        trapDropChance = HUDManager.dropChance.value;
        UpdateEnemyStats();
        

    } 

    private void SpawnEnemies()
    {
        if(enemiesToSpawn > 0)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                enemy = Instantiate(EnemyPrefab).GetComponent<BaseEnemy>();
                enemyList.Add(enemy);
                enemy.PositionCharacter(GridManager.Instance.GetRandomSpawnTile());
                enemy.CalculateRange();
            }
        }
    }

    private void SpawnPotions()
    {
        if (potionsToSpawn > 0)
        {
            for (int i = 0; i < potionsToSpawn; i++)
            {
                var potion = Instantiate(PotionPrefab);
                potionList.Add(potion);
                PositionItem(GridManager.Instance.GetRandomSpawnTile(), potion);
            }
        }
    }


    public void UpdateState()
    {
        switch (turnState)
        {
            case TurnState.Processing:
                ProcessTurns();
                break;
            case TurnState.PlayerTurn:
                PlayerTurn();
                break; 
            case TurnState.EnemyTurn:
                EnemyTurn();
                break;
            case TurnState.GameOver:
                break;
            default:
                break;
        }

        UpdateUI();
    }

    private void PlayerTurn()
    {
        playerChar.state = AI_Player.State.Evaluate;
        playerChar.UpdateState();
        if(playerChar.hasPackage)
        {
            UpdateScore(5);
        }
  
    }

    public void ProcessTurns()
    {
        //Agent successfully reached end

        if(playerChar!= null)
        {
            if (endTileReached)
            {
                if (playerChar.hasPackage)
                {
                    UpdateScore(deliveredScore);
                    EndGame("Package Delivered - Mission Success");
                    return;
                }
                else
                {
                    UpdateScore(survivedScore);
                    EndGame("Agent survived but package not delivered - Mission Failure");
                    return;
                }
            }

        }

        //Agent ran out of turns to complete mission
        if (turnCount <=0 && lifeCount > 0)
        {
            UpdateScore(turnScore);
            ResetPlayer();
            turnState = TurnState.PlayerTurn;
            UpdateState();
            return;

        } else if(turnCount <=0 && lifeCount <=0)
        {
            EndGame("Ran out of turns - Mission Failure");
            return;
        }

        //Handle Agent death and respawn on life counter
        if(playerChar.isDead && lifeCount > 0)
        {
            UpdateScore(deathScore);
            ResetPlayer();
            playerChar.hasPackage = false;
            turnState = TurnState.PlayerTurn;
            UpdateState();
            return;
        }
        else if (playerChar.isDead && lifeCount <= 0)
        {
            EndGame("Ran out of lives - Mission Failure");
            return;
        }

        turnCount--;
        turnState = TurnState.PlayerTurn;
        UpdateState();
        UpdateUI();
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

    public void UpdateEnemyStats()
    {
        if (enemyList.Count > 0)
        {
            foreach (var enemy in enemyList)
            {
                enemy.range = HUDManager.enemiesRange.value;
                enemy.attackDamage = HUDManager.enemyDamage.value;

            }
        }
    }

    public void SpawnCharacter(OverlayInfo start)
    {
        playerChar = Instantiate(AIPlayerPrefab).GetComponent<AI_Player>();
        playerChar.PositionCharacter(start);
        playerChar.CalculateRange();
    }


    public void EndGame(string condition)
    {
        HUDManager.UpdateGameOver(condition);
        turnState = TurnState.GameOver;
        UpdateUI();
        UpdateState();
        HUDManager.restart.SetEnabled(true);
        
    }

    public void ResetSim()
    {
        ResetPlayer();
        if (enemyList.Count > 0)
        {
            foreach (var enemy in enemyList)
            {
                Destroy(enemy.gameObject);

            }
            enemyList.Clear();
        }

        if(potionList.Count > 0)
        {
            foreach(var potion in potionList)
            {
                Destroy(potion);
            }
            potionList.Clear();
        }

        if (dropTrapList.Count > 0)
        {
            foreach (var trap in dropTrapList)
            {
                Destroy(trap);
            }
            dropTrapList.Clear();
        }

        GridManager.Instance.ResetAllOverlays();
        endTileReached= false;
        HUDManager.restart.SetEnabled(false);
        HUDManager.UpdateGameOver("");

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
            playerChar.currentHealth -= damage;
            playerChar.healthBar.value = playerChar.currentHealth;
        }
        else
        {
            KillPlayer();
        }

    }

    public void HealPlayer(int healAmount)
    {
        playerChar.currentHealth += healAmount;
        playerChar.healthBar.value = playerChar.currentHealth;
        if (playerChar.currentHealth < agentMaxHealth)
        {
            playerChar.currentHealth = agentMaxHealth;
            playerChar.healthBar.value = playerChar.currentHealth;
        }
    }
    public void ResetPlayer()
    {

        playerChar.currentHealth = agentMaxHealth;
        playerChar.healthBar.value = playerChar.currentHealth;
        playerChar.isDead = false;
        playerChar.spriteRenderer.sprite = livingPlayer;
        playerChar.PositionCharacter(startTile);
        lifeCount--;
        turnCount = maxTurnCount;

    }

    public void KillPlayer()
    {
        playerChar.currentHealth = 0;
        playerChar.healthBar.value = playerChar.currentHealth;
        playerChar.isDead = true;
        playerChar.spriteRenderer.sprite = deadPlayer;  
        if(playerChar.hasPackage)
        {
            Instantiate(packagePrefab, playerChar.activeTile.transform.position, Quaternion.identity);
            packageTile = playerChar.activeTile;
        }
    }

    public string PackageState()
    {
        var temp = "";

        if (playerChar.hasPackage)
        {
            temp = "Agent Possession";
        }
        else if (packageTile != null)
        {
            temp = "Dropped";
        }
        else
        {
            temp = "Enemy Possession";
        }

        return temp;
    }

    public void UpdateScore(int scoreInc)
    {
            score += scoreInc;
    }

    public void UpdateUI()
    {
        HUDManager.UpdateUI(turnCount.ToString(), score.ToString(), lifeCount.ToString(), turnState.ToString(), PackageState());
    }
    public void PositionItem(OverlayInfo tile, GameObject item)
    {
        item.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        item.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;

    }
}
