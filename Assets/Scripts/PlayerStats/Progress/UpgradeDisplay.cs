using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _nextLevelDescription;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private Image _thumbnail;
    [SerializeField] private Button _upgradeButton;

    [SerializeField] private UpgradeProgress _upgradeProgress;
    [SerializeField] private Save _save;
    [SerializeField] private Score _score;
    private UpgradesPanel _upgradesPanel;

    void Start()
    {
        if (_upgradeProgress != null)
        {
            UpdateInfo();
        }

        _upgradesPanel = FindObjectOfType<UpgradesPanel>();
    }

    public void UpdateInfo()
    {
        _name.text = _upgradeProgress.UpgradeName;
        _description.text = _upgradeProgress.Description;
        _nextLevelDescription.text = _upgradeProgress.NextLevelDescription;
        _price.text = _upgradeProgress.IsMaxed ? "Max" : _upgradeProgress.GetNextPrice().ToString();
        _thumbnail.sprite = _upgradeProgress.Icon;
        if (!_upgradeProgress.IsMaxed && _score.ComboTotalScore >= _upgradeProgress.GetNextPrice())
        {
            _upgradeButton.interactable = true;
        }
        else
        {
            _upgradeButton.interactable = false;
        }
    }

    public void BuyUpgrade()
    {
        int price = (int)_upgradeProgress.GetNextPrice();
        int current = _score.ComboTotalScore;
        _score.ConsumeComboPoints(price);
        _upgradeProgress.LevelUp();
        _upgradesPanel.UpdateAllUpgradeDisplays();
        _upgradesPanel.ConsumeComboPoints(price, current);
        _save.SavePlayerPrefs();
    }
    


}
