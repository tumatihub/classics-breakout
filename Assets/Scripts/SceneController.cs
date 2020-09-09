using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNames
{
    public const string Game = "Sandbox";
}

public class SceneController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneNames.Game);
    }
}
