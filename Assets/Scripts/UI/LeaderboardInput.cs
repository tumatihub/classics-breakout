using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LeaderboardInput : MonoBehaviour
{
    private Leaderboard _leaderboard;
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private Button _button;
    [SerializeField] private Score _score;
    [SerializeField] private string _saveRecordSuccessMsg;
    [SerializeField] private string _saveRecordFailureMsg;
    [SerializeField] private string _savePendingRecordMsg;
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
        if (HasPendingRecord()) InsertPendingRecord();
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
        SavePendingRecord();
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

    private bool HasPendingRecord()
    {
        int pending = PlayerPrefs.GetInt(PrefsKeys.HAS_PENDING_RECORD);
        return (pending == 0) ? false : true;
    }

    private void SavePendingRecord()
    {
        PlayerPrefs.SetInt(PrefsKeys.HAS_PENDING_RECORD, 1);
        PlayerPrefs.SetInt(PrefsKeys.PENDING_TOTAL_COMBO, _score.ComboTotalScore);
        PlayerPrefs.SetInt(PrefsKeys.PENDING_MAX_COMBO, _score.MaxCombo);
        PlayerPrefs.SetInt(PrefsKeys.PENDING_SCORE, _score.TotalScore);
        PlayerPrefs.SetString(PrefsKeys.PENDING_NAME, _input.text);
    }

    private void InsertPendingRecord()
    {
        var entry = new LeaderboardEntry(
            PlayerPrefs.GetString(PrefsKeys.PENDING_NAME),
            PlayerPrefs.GetInt(PrefsKeys.PENDING_TOTAL_COMBO),
            PlayerPrefs.GetInt(PrefsKeys.PENDING_MAX_COMBO),
            PlayerPrefs.GetInt(PrefsKeys.PENDING_SCORE)
        );
        _leaderboard.InsertEntry(entry, HandlePendingInsertSuccess, null);
    }

    private void HandlePendingInsertSuccess()
    {
        PlayerPrefs.SetInt(PrefsKeys.HAS_PENDING_RECORD, 0);
        _panelNotification?.NotifySuccess(_savePendingRecordMsg);
    }
}
