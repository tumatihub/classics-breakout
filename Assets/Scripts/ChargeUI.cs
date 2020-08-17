using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChargeUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] TMP_Text _chargeValueDisplay;
    [SerializeField] TMP_Text _specialName;

    private void OnEnable()
    {
        _playerStats.ChargeUpEvent += UpdateChargeValue;
        _playerStats.ChargeConsumeEvent += UpdateChargeValue;
        _playerStats.ChangeSpecialEvent += ChangeSpecial;
        _playerStats.ChargeResetEvent += UpdateChargeValue;
    }

    private void UpdateChargeValue()
    {
        _chargeValueDisplay.text = _playerStats.Charge.ToString();
    }

    private void ChangeSpecial()
    {
        _specialName.text = _playerStats.Special.Name;
        _chargeValueDisplay.color = _playerStats.Special.Color;
        _specialName.color = _playerStats.Special.Color;
    }

    private void OnDisable()
    {
        _playerStats.ChargeUpEvent -= UpdateChargeValue;
        _playerStats.ChargeConsumeEvent -= UpdateChargeValue;
        _playerStats.ChangeSpecialEvent -= ChangeSpecial;
        _playerStats.ChargeResetEvent -= UpdateChargeValue;
    }
}
