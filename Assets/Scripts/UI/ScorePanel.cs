using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        _upgradesNotification.SetValue(GetAvailableUpgrades());
        UpdateInfo();
        _save.SavePlayerPrefs();
    }

    private void UpdateInfo()
    {
        _comboPointsValue.text = _score.ComboTotalScore.ToString();
        _totalScoreValue.text = _score.TotalScore.ToString();
        _maxCombo.text = _score.MaxCombo.ToString();
        _maxComboRecord.text = _score.MaxComboRecord.ToString();
        _comboTotalRecord.text = _score.ComboTotalRecord.ToString();
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
