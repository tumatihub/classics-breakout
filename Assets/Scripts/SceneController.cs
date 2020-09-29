using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNames
{
    public const string Game = "Sandbox";
    public const string MainMenu = "MainMenu";
    public const string Upgrades = "Upgrades";
    public const string Score = "Score";
}

public class SceneController : MonoBehaviour
{
    [SerializeField] private Transition _transition;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    public void TransitionToStartGame()
    {
        _transition.RunExit(StartGame);
    }

    public void StartGame()
    {
        _audioManager.TransitionToGame();
        SceneManager.LoadScene(SceneNames.Game);
    }

    public void TransitionToUpgrades()
    {
        _transition.RunExit(LoadUpgradesScreen);
    }

    public void LoadUpgradesScreen()
    {
        SceneManager.LoadScene(SceneNames.Upgrades);
    }

    public void TransitionToMainMenu()
    {
        _transition.RunExit(LoadMainMenu);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(SceneNames.MainMenu);
    }

    public void TransitionToScoreScreen()
    {
        _audioManager.TransitionToMenu();
        _transition.RunExit(LoadScoreScreen);
    }

    public void LoadScoreScreen()
    {
        SceneManager.LoadScene(SceneNames.Score);
    }
}
