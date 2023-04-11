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

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        Score = root.Q<Label>("label-score");
        Turn = root.Q<Label>("label-turn");
        Quit = root.Q<Button>("button-quit");
    }

    private void Start()
    {
        Turn.text = "Turn: " + GameManager.Instance.maxTurnCount.ToString() + "/" + GameManager.Instance.maxTurnCount.ToString();
        Score.text = "Score: 0";
        Quit.clickable.clicked += () => SceneManager.LoadScene("MainMenu");
    }

    public void UpdateUI(string turn, string score)
    {
        Turn.text = "Turn: " + turn + "/" + GameManager.Instance.maxTurnCount.ToString();
        Score.text = "Score: " + score;
    }
}
