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
    private Button Quit;
    public SliderInt enemySlider;
    public IntegerField agentHealth;
    public IntegerField trapDamage;
    public IntegerField healing;
    public SliderInt agentRange;
    public SliderInt enemiesRange;
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
        Turn = root.Q<Label>("label-turn");
        Quit = root.Q<Button>("button-quit");
        agentHealth = root.Q<IntegerField>("hud-integerfield-health");
        trapDamage = root.Q<IntegerField>("hud-integerfield-trap");
        enemyDamage = root.Q<IntegerField>("hud-integerfield-enemydp");
        healing = root.Q<IntegerField>("hud-integerfield-healing");
        turns = root.Q<IntegerField>("hud-integerfield-turns");
        lives = root.Q<IntegerField>("hud-integerfield-lives");
        agentRange = root.Q<SliderInt>("hud-sliderint-range");
        enemySlider = root.Q<SliderInt>("hud-sliderint-enemies");
        enemiesRange = root.Q<SliderInt>("hud-sliderint-enemiesrange");
        start = root.Q<Button>("hud-button-start");
        restart = root.Q<Button>("hud-button-restart");
        debug = root.Q<Toggle>("hud-toggle-debug");
        start.clickable.clicked += () => StartSim();
        restart.clickable.clicked += () => Restart();
        restart.SetEnabled(false);
    }

    private void Start()
    {

        Quit.clickable.clicked += () => SceneManager.LoadScene("MainMenu");
    }

    public void UpdateUI(string turn, string score)
    {
        Turn.text = "Turn: " + turn + "/" + GameManager.Instance.maxTurnCount.ToString();
        Score.text = "Score: " + score;
    }

    public void StartSim()
    {
        GameManager.Instance.StartSimulation();
        Turn.text = "Turn: " + GameManager.Instance.maxTurnCount.ToString() + "/" + GameManager.Instance.maxTurnCount.ToString();
        Score.text = "Score: 0";
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
    }

    public void Restart()
    {
        ToggleEnabled(true);
        GameManager.Instance.ResetSim();
    }
}
