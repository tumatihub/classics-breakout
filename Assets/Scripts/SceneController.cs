using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNames
{
    public const string Game = "Sandbox";
    public const string MainMenu = "MainMenu";
    public const string Upgrades = "Upgrades";
}

public class SceneController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneNames.Game);
    }

    public void LoadUpgradesScreen()
    {
        SceneManager.LoadScene(SceneNames.Upgrades);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(SceneNames.MainMenu);
    }
}
