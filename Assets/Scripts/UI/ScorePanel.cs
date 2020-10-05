using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private Notification _upgradesNotification;
    [SerializeField] private Score _score;
    [SerializeField] private Save _save;
    [SerializeField] private Upgrades _upgrades;
    [SerializeField] private TMP_Text _comboPointsValue;
    [SerializeField] private TMP_Text _totalScoreValue;
    [SerializeField] private TMP_Text _maxCombo;
    [SerializeField] private TMP_Text _maxComboRecord;
    [SerializeField] private TMP_Text _comboTotalRecord;
    [SerializeField] private GameObject _recordImage;
    [SerializeField] private Color _recordColor;
    [SerializeField] private LeaderboardInput _leaderboardInput;

    private bool _hasRecord;
    private int _lastComboTotalScore;
    private int _lastMaxCombo;

    private void Start()
    {
        _upgradesNotification.SetValue(GetAvailableUpgrades());
        _lastComboTotalScore = PlayerPrefs.GetInt(PrefsKeys.TOTAL_COMBO);
        _lastMaxCombo = PlayerPrefs.GetInt(PrefsKeys.MAX_COMBO);

        UpdateInfo();
        _save.SavePlayerPrefs();
    }

    private void UpdateInfo()
    {
        if (_score.ComboTotalScore > _lastComboTotalScore)
        {
            _hasRecord = true;
            _comboPointsValue.color = _recordColor;
            _comboTotalRecord.color = _recordColor;
        }
        _comboPointsValue.text = _score.ComboTotalScore.ToString();
        _comboTotalRecord.text = _score.ComboTotalRecord.ToString();

        if (_score.MaxCombo > _lastMaxCombo)
        {
            _hasRecord = true;
            _maxCombo.color = _recordColor;
            _maxComboRecord.color = _recordColor;
        }
        _maxCombo.text = _score.MaxCombo.ToString();
        _maxComboRecord.text = _score.MaxComboRecord.ToString();

        _totalScoreValue.text = _score.TotalScore.ToString();

        if (_hasRecord)
        {
            ShowRecordImage();
            ShowLeaderboardInput();
        }
    }

    private void ShowLeaderboardInput()
    {
        _leaderboardInput.gameObject.SetActive(true);
    }

    public void ClearSave()
    {
        _save.ClearSave();
    }

    private void ShowRecordImage()
    {
        _recordImage.SetActive(true);
        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.scale(_recordImage, new Vector3(1.2f, 1.2f, 1f), 1f)
        );
        seq.append(
            LeanTween.scale(_recordImage, new Vector3(1f, 1f, 1f), .5f)
        );
    }

    public int GetAvailableUpgrades()
    {
        int qty = 0;
        foreach (var upgrade in _upgrades.AllUpgrades)
        {
            if (upgrade.CanUpgrade(_score.ComboTotalScore)) qty++;
        }
        return qty;
    }
}
