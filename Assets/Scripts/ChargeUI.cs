using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChargeUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] TMP_Text _chargeValueDisplay;

    private void OnEnable()
    {
        _playerStats.ChargeUpEvent += UpdateChargeValue;
    }

    private void UpdateChargeValue()
    {
        _chargeValueDisplay.text = _playerStats.Charge.ToString();
    }

    private void OnDisable()
    {
        _playerStats.ChargeUpEvent -= UpdateChargeValue;
    }
}
