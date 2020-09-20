using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialOption : MonoBehaviour
{
    private Special _special;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _icon;

    public void SetSpecial(Special special)
    {
        _special = special;
    }

    public void UpdateDisplay()
    {
        _name.text = _special.Name;
        _icon.sprite = _special.Icon;
    }
}
