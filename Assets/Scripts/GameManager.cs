using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public bool Debugging;

    public GameObject AIPlayerPrefab;
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

    public void SpawnCharacter(OverlayInfo start)
    {
        playerChar = Instantiate(AIPlayerPrefab).GetComponent<AI_Player>();
        playerChar.PositionCharacter(start);
        playerChar.CalculateRange();
    }

    public void Win(GameObject player)
    {
        Debug.Log("Reached end tile!");
        player.SetActive(false);
    }


}
