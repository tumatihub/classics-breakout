using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradesPanel : MonoBehaviour
{
    private List<UpgradeDisplay> _upgradeDisplays = new List<UpgradeDisplay>();
    [SerializeField] private Upgrades _upgrades;
    [SerializeField] private TMP_Text _comboPointsValueDisplay;
    [SerializeField] private Score _score;

    void Start()
    {
        _upgradeDisplays = new List<UpgradeDisplay>(FindObjectsOfType<UpgradeDisplay>());
        UpdateComboPointsDisplay();
    }

    public void UpdateAllUpgradeDisplays()
    {
        foreach(var upgrade in _upgradeDisplays)
        {
            upgrade.UpdateInfo();
        }
    }

    public void UpdateComboPointsDisplay()
    {
        _comboPointsValueDisplay.text = _score.ComboTotalScore.ToString();
    }

    public void ResetUpgrades()
    {
        _upgrades.ResetUpgrades();
        UpdateAllUpgradeDisplays();
        UpdateComboPointsDisplay();
    }
}
