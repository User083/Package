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

        
        agentStateInfo = root.Q<Label>("label-playerstate");
        enemyStateInfo = root.Q<Label>("label-enemystate");
        PackageInfo = root.Q<Label>("label-package");
        gameOverInfo = root.Q<Label>("label-gameover");



    }


    public void UpdateUI(string playerState, string enemyState, string hasPackage)
    {
        

        agentStateInfo.text = "Agent State: " + playerState;
        enemyStateInfo.text = "Enemy State: " + enemyState;

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

