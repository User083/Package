using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private Button Quit;
    private Button StartButton;
    private VisualElement root;


    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        Quit = root.Q<Button>("quit-button");
        StartButton = root.Q<Button>("start-button");
    }

    private void Start()
    {
        StartButton.clickable.clicked += () => LoadSim();
        Quit.clickable.clicked += () => Application.Quit();
    }

    private void LoadSim()
    {
        SceneManager.LoadSceneAsync("TestScene");
    }
}
