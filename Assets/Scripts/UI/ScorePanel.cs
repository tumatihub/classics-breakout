using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePanel : MonoBehaviour
{
    [SerializeField] private Notification _upgradesNotification;
    [SerializeField] private Score _score;
    [SerializeField] private Upgrades _upgrades;
    [SerializeField] private TMP_Text _comboPointsValue;
    [SerializeField] private TMP_Text _totalScoreValue;

    private void Start()
    {
        _upgradesNotification.SetValue(GetAvailableUpgrades());
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        _comboPointsValue.text = _score.ComboTotalScore.ToString();
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
