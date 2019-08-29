using CGJ.System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startGame;
    public Button quit;

    private void Start()
    {
        startGame.onClick.AddListener(StartGame);
        quit.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void StartGame()
    {
        SystemManager.systems.sceneLoadingSystem.LoadSceneByIndex(2);
    }
}
