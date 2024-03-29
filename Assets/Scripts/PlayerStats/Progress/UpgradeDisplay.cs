﻿using System.Collections;
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
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private Image _costBG;

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

        if (_upgradeProgress.Special == null) _costBG.gameObject.SetActive(false);
    }

    public void UpdateInfo()
    {
        _name.text = _upgradeProgress.UpgradeName;
        _description.text = _upgradeProgress.Description;
        _nextLevelDescription.text = _upgradeProgress.NextLevelDescription;
        _price.text = _upgradeProgress.IsMaxed ? "Max" : _upgradeProgress.GetNextPrice().ToString();
        _thumbnail.sprite = _upgradeProgress.Icon;
        _cost.text = _upgradeProgress.Special == null ? "--" : $"Cost: {_upgradeProgress.Special.ChargeCost}";
        if (!_upgradeProgress.IsMaxed && _score.ComboTotalScore >= _upgradeProgress.GetNextPrice())
        {
            EnableButton();
        }
        else
        {
            DisableButton();
        }
    }

    private void DisableButton()
    {
        _upgradeButton.interactable = false;
        _upgradeButton.GetComponent<Image>().raycastTarget = false;
    }

    private void EnableButton()
    {
        _upgradeButton.interactable = true;
        _upgradeButton.GetComponent<Image>().raycastTarget = true;
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
