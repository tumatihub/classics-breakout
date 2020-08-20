﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelProperty : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _propertyNameDisplay;
    [SerializeField] private TMP_Text _propertyValueDisplay;
    [SerializeField] private TMP_Text _minValueDisplay;
    [SerializeField] private TMP_Text _maxValueDisplay;
    [SerializeField] private FloatVariable _property;
    [SerializeField] bool _isInteger;

    private void Awake()
    {
        _propertyNameDisplay.text = _name;
        _slider.maxValue = _maxValue;
        _slider.value = _property.Value;
        _slider.minValue = _minValue;
        _propertyValueDisplay.text = _property.Value.ToString();
        _slider.wholeNumbers = _isInteger;

        _minValueDisplay.text = _minValue.ToString();
        _maxValueDisplay.text = _maxValue.ToString();

    }
    private void Start()
    {
    }

    public void UpdateProperty()
    {
        _propertyValueDisplay.text = _slider.value.ToString();
        _property.Value = _slider.value;
    }
}
