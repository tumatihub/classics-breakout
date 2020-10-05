using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardInput : MonoBehaviour
{
    private Leaderboard _leaderboard;
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private Button _button;
    [SerializeField] private Score _score;

    void Start()
    {
        _leaderboard = FindObjectOfType<Leaderboard>();
        _button.interactable = false;
        _button.image.raycastTarget = false;
    }

    public void DisableButtonIfInputIsNull()
    {
        if (_input.text.Length == 0)
        {
            _button.interactable = false;
            _button.image.raycastTarget = false;
        }
        else
        {
            _button.interactable = true;
            _button.image.raycastTarget = true;
        }
    }

    public void SaveRecord()
    {
        var entry = new LeaderboardEntry(_input.text, _score.ComboTotalScore, _score.MaxCombo, _score.TotalScore);
        _leaderboard.InsertEntry(entry, HandleInsertSuccess, HandleInsertFailure);
    }

    private void HandleInsertSuccess()
    {
        gameObject.SetActive(false);
    }

    private void HandleInsertFailure(string error)
    {
        Debug.LogError(error);
    }
}
