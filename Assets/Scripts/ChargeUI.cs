using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChargeUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] TMP_Text _specialName;
    [SerializeField] Image _progressBar;
    private float _slideBarMaxWidth = 100f;

    private void OnEnable()
    {
        _playerStats.ChargeUpEvent += UpdateChargeValue;
        _playerStats.ChargeConsumeEvent += UpdateChargeValue;
        _playerStats.ChangeSpecialEvent += ChangeSpecial;
        _playerStats.ChargeResetEvent += UpdateChargeValue;
    }

    private void UpdateChargeValue()
    {
        var rect = _progressBar.rectTransform;
        rect.sizeDelta = new Vector2(Mathf.Lerp(0, _slideBarMaxWidth, (float)_playerStats.Charge / (float)_playerStats.ChargeMax), rect.sizeDelta.y);
    }

    private void ChangeSpecial()
    {
        _specialName.text = _playerStats.Special.Name;
        _specialName.color = _playerStats.Special.Color;
        _progressBar.color = _playerStats.Special.Color;
    }

    private void OnDisable()
    {
        _playerStats.ChargeUpEvent -= UpdateChargeValue;
        _playerStats.ChargeConsumeEvent -= UpdateChargeValue;
        _playerStats.ChangeSpecialEvent -= ChangeSpecial;
        _playerStats.ChargeResetEvent -= UpdateChargeValue;
    }
}
