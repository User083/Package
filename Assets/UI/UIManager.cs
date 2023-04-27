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
    private Label PackageInfo;
    private VisualElement root;

    private void Awake()
    {
        DebugDoc= GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        root = DebugDoc.rootVisualElement;

        turnInfo = root.Q<Label>("label-turn");
        gameStateInfo = root.Q<Label>("label-gamestate");
        agentStateInfo = root.Q<Label>("label-playerstate");
        enemyStateInfo = root.Q<Label>("label-enemystate");
        agentHealthInfo = root.Q<Label>("label-playerhealth");
        agentLivesInfo = root.Q<Label>("label-playerlives");
        PackageInfo = root.Q<Label>("label-package");
        gameOverInfo = root.Q<Label>("label-gameover");



    }


    public void UpdateUI(string turn, string gameState, string playerState, string enemyState, string health, string lifeCount, string hasPackage)
    {
        
        turnInfo.text = "Turns Remaining: " + turn;
        gameStateInfo.text = "Game State: " + gameState;
        agentStateInfo.text = "Agent State: " + playerState;
        enemyStateInfo.text = "Enemy State: " + enemyState;
        agentHealthInfo.text = "Agent Health: " + health + "/" + GameManager.Instance.agentMaxHealth.ToString();
        agentLivesInfo.text = "Agent Lives: " + lifeCount + "/" + GameManager.Instance.maxLifeCount.ToString();
        PackageInfo.text = "Package State: " + hasPackage;

    }

    public void UpdateGameOver(string gameOver)
    {
        gameOverInfo.text = "Game Over: " + gameOver;
    }

    public void ToggleVisibility(bool state)
    {
        root.visible = state;
    }
}

