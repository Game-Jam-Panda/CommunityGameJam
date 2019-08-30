using CGJ.System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button startGame;
    [SerializeField] int sceneIndexToLoadOnStartGame = 2;
    [SerializeField] Button quit;

    [Header("Sound Effects")]
    [SerializeField] AudioClip buttonClickSE = null;

    private void Start()
    {
        startGame.onClick.AddListener(StartGame);
        quit.onClick.AddListener(QuitGame);
    }

    private void QuitGame()
    {
        SystemManager.systems.soundManager.PlaySound(buttonClickSE);

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    private void StartGame()
    {
        SystemManager.systems.soundManager.PlaySound(buttonClickSE);

        SystemManager.systems.sceneLoadingSystem.LoadSceneByIndex(sceneIndexToLoadOnStartGame);
    }
}
