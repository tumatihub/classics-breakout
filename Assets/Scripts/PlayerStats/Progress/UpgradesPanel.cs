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
    [SerializeField] private TMP_Text _removedPoints;

    void Start()
    {
        _upgradeDisplays = new List<UpgradeDisplay>(FindObjectsOfType<UpgradeDisplay>());
        _removedPoints.alpha = 0;
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

    public void ConsumeComboPoints(int pointsToConsume, int totalCurrent)
    {
        _removedPoints.text = $"-{pointsToConsume}";
        LeanTween.value(_removedPoints.gameObject, UpdateAlpha, 1f, 0f, 1f).setEase(LeanTweenType.easeInCirc);
        LeanTween.value(_comboPointsValueDisplay.gameObject, UpdateComboPointsToValue, totalCurrent, totalCurrent-pointsToConsume, 1f).setEase(LeanTweenType.easeOutCirc);
    }

    void UpdateComboPointsToValue(float value)
    {
        int intValue = (int)value;
        _comboPointsValueDisplay.text = intValue.ToString();
    }

    void UpdateAlpha(float value)
    {
        _removedPoints.alpha = value;
    }

    public void ResetUpgrades()
    {
        _upgrades.ResetUpgrades();
        UpdateAllUpgradeDisplays();
        UpdateComboPointsDisplay();
    }

    public void MaxUpgrades()
    {
        _upgrades.MaxUpgrades();
        UpdateAllUpgradeDisplays();
        UpdateComboPointsDisplay();
    }
}
