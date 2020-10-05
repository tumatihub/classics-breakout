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
    [SerializeField] private string _saveRecordSuccessMsg;
    [SerializeField] private string _saveRecordFailureMsg;
    [SerializeField] private Transform _showPos;
    [SerializeField] private Transform _hidePos;
    [SerializeField] private float _moveDelay = .5f;
    [SerializeField] private GameObject _loadingImg;
    private PanelNotification _panelNotification;

    void Start()
    {
        _panelNotification = FindObjectOfType<PanelNotification>();
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
        _loadingImg.SetActive(true);
        var entry = new LeaderboardEntry(_input.text, _score.ComboTotalScore, _score.MaxCombo, _score.TotalScore);
        _button.interactable = false;
        _button.image.raycastTarget = false;
        _leaderboard.InsertEntry(entry, HandleInsertSuccess, HandleInsertFailure);
    }

    private void HandleInsertSuccess()
    {
        _loadingImg.SetActive(false);
        _panelNotification?.NotifySuccess(_saveRecordSuccessMsg);
        HideInput();
    }

    private void HandleInsertFailure(string error)
    {
        _loadingImg.SetActive(false);
        _panelNotification?.NotifyFailure(_saveRecordFailureMsg);
        HideInput();
        Debug.LogError(error);
    }

    public void ShowInput()
    {
        LeanTween.move(gameObject, _showPos, _moveDelay).setEase(LeanTweenType.easeOutCirc);
    }

    public void HideInput()
    {
        LeanTween.move(gameObject, _hidePos, _moveDelay).setEase(LeanTweenType.easeInCirc);
    }
}
