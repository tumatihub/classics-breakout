using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SceneNames
{
    public const string Game = "Sandbox";
    public const string MainMenu = "MainMenu";
    public const string Upgrades = "Upgrades";
    public const string Score = "Score";
    public const string Leaderboard = "Leaderboard";
    public const string Credits = "Credits";
}

public class SceneController : MonoBehaviour
{
    [SerializeField] private Transition _transition;
    private AudioManager _audioManager;
    [SerializeField] private Upgrades _upgrades;

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

    public void TransitionToLeaderboard()
    {
        _transition.RunExit(LoadLeaderboard);
    }

    public void LoadLeaderboard()
    {
        SceneManager.LoadScene(SceneNames.Leaderboard);
    }

    public void TransitionToCredits()
    {
        _transition.RunExit(LoadCredits);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(SceneNames.Credits);
    }

    public void TransitionToMainMenu()
    {
        _audioManager.TransitionToMainMenu();
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

    public void Quit()
    {
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        Dictionary<string, object> upgradesParams = new Dictionary<string, object>();
        foreach (var upgrade in _upgrades.AllUpgrades)
        {
            upgradesParams.Add(upgrade.UpgradeName, upgrade.Level);
        }
        AnalyticsEvent.Custom("upgrades_level", upgradesParams);
    }
}
