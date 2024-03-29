using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    private VisualElement root;
    private Label Score;
    private Label Turn;
    private Label Lives;
    private Label State;
    private Label Package;
    private Label GameOver;
    private Label AgentState;
    private Button Quit;
    public SliderInt enemySlider;
    public IntegerField agentHealth;
    public IntegerField trapDamage;
    public IntegerField healing;
    public SliderInt agentRange;
    public SliderInt potions;
    public SliderInt dropChance;
    public SliderInt enemiesRange;
    public SliderInt turnChance;
    public SliderInt healthChance;
    private Button start;
    public IntegerField turns;
    public IntegerField lives;
    public Toggle debug;
    public Button restart;
    public IntegerField enemyDamage;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        Score = root.Q<Label>("label-score");
        AgentState = root.Q<Label>("label-agentstate");
        Turn = root.Q<Label>("label-turn");
        Lives = root.Q<Label>("label-lives");
        GameOver = root.Q<Label>("label-gameover");
        State = root.Q<Label>("label-state");
        Package = root.Q<Label>("label-package");
        Quit = root.Q<Button>("button-quit");
        agentHealth = root.Q<IntegerField>("hud-integerfield-health");
        trapDamage = root.Q<IntegerField>("hud-integerfield-trap");
        enemyDamage = root.Q<IntegerField>("hud-integerfield-enemydp");
        healing = root.Q<IntegerField>("hud-integerfield-healing");
        turns = root.Q<IntegerField>("hud-integerfield-turns");
        lives = root.Q<IntegerField>("hud-integerfield-lives");
        agentRange = root.Q<SliderInt>("hud-sliderint-range");
        dropChance = root.Q<SliderInt>("hud-sliderint-drop");
        turnChance = root.Q<SliderInt>("hud-sliderint-turnsleft");
        healthChance = root.Q<SliderInt>("hud-sliderint-healthleft");
        enemySlider = root.Q<SliderInt>("hud-sliderint-enemies");
        enemiesRange = root.Q<SliderInt>("hud-sliderint-enemiesrange");
        potions = root.Q<SliderInt>("hud-sliderint-potionno");
        start = root.Q<Button>("hud-button-start");
        restart = root.Q<Button>("hud-button-restart");
        debug = root.Q<Toggle>("hud-toggle-debug");
        start.clickable.clicked += () => StartSim();
        restart.clickable.clicked += () => Restart();
       
    }

    private void Start()
    {

        Quit.clickable.clicked += () => SceneManager.LoadScene("MainMenu");
    }

    public void ResetUI()
    {
        Turn.text = "Turn: ";
        Score.text = "Score: ";
        Lives.text = "Lives: ";
        State.text = "Turn State: ";
        Package.text = "Package: ";
        AgentState.text = "Agent State:";
    }

    public void UpdateUI(string turn, string score, string lives, string state, string package, string agentState)
    {
        Turn.text = "Turn: " + turn + "/" + GameManager.Instance.maxTurnCount.ToString();
        Score.text = "Score: " + score;
        Lives.text = "Lives: " + lives + "/" + GameManager.Instance.maxLifeCount.ToString(); 
        State.text = "Turn State: " + state;
        Package.text = "Package: " + package;
        AgentState.text = "Agent State: " + agentState;
    }

    public void StartSim()
    {
        GameManager.Instance.StartSimulation();
        ToggleEnabled(false);
    }

    public void ToggleEnabled(bool toggle)
    {
        turns.SetEnabled(toggle);
        lives.SetEnabled(toggle);
        healing.SetEnabled(toggle);
        agentHealth.SetEnabled(toggle);
        trapDamage.SetEnabled(toggle);
        enemiesRange.SetEnabled(toggle);
        agentRange.SetEnabled(toggle);
        enemySlider.SetEnabled(toggle);
        enemyDamage.SetEnabled(toggle);
        start.SetEnabled(toggle);
        debug.SetEnabled(toggle);
        dropChance.SetEnabled(toggle);
        potions.SetEnabled(toggle);
        turnChance.SetEnabled(toggle);
        healthChance.SetEnabled(toggle);
    }

    public void Restart()
    {
        ToggleEnabled(true);
        GameManager.Instance.ResetSim();
    }

    public void UpdateGameOver(string condition)
    {
        GameOver.text = condition;
        if(condition == "")
        {
            GameOver.style.backgroundColor = new Color(0, 0, 0, 0);
        }
        else
        {
            GameOver.style.backgroundColor = new Color(0, 0, 0, 0.3f);
        }
        
    }
}
