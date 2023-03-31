using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIDocument DebugDoc;

    private Label turnInfo;
    private Label gameStateInfo;
    private Label agentStateInfo;
    private Label enemyStateInfo;
    private Label agentHealthInfo;
    private Label agentLivesInfo;
    private Label gameOverInfo;

    private void Awake()
    {
        DebugDoc= GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = DebugDoc.rootVisualElement;

        turnInfo = root.Q<Label>("label-turn");
        gameStateInfo = root.Q<Label>("label-gamestate");
        agentStateInfo = root.Q<Label>("label-playerstate");
        enemyStateInfo = root.Q<Label>("label-enemystate");
        agentHealthInfo = root.Q<Label>("label-playerhealth");
        agentLivesInfo = root.Q<Label>("label-playerlives");
        gameOverInfo = root.Q<Label>("label-gameover");
    }

    public void UpdateUI(string turn, string gameState, string playerState, string enemyState, string health, string lifeCount)
    {
        turnInfo.text = "Turn: " + turn;
        gameStateInfo.text = "Game State: " + gameState;
        agentStateInfo.text = "Agent State: " + playerState;
        enemyStateInfo.text = "Enemy State: " + enemyState;
        agentHealthInfo.text = "Agent Health: " + health + "/100";
        agentLivesInfo.text = "Agent Lives: " + lifeCount + "/3";

    }

    public void UpdateGameOver(string gameOver)
    {
        gameOverInfo.text = "Game Over: " + gameOver;
    }
}

