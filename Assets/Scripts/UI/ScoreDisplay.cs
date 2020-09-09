using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _comboCounterValue;
    [SerializeField] private TMP_Text _totalComboValue;
    [SerializeField] private Score _score;

    private void Start()
    {
        HideComboCounter();
        UpdateTotalComboDisplay();    
    }

    public void ShowComboCounter()
    {
        _comboCounterValue.alpha = 1f;
    }

    public void HideComboCounter()
    {
        _comboCounterValue.alpha = 0f;
    }

    public void UpdateComboCounterDisplay()
    {
        _comboCounterValue.text = $"{_score.ComboCounter}x combo";
    }

    public void UpdateTotalComboDisplay()
    {
        _totalComboValue.text = _score.ComboTotalScore.ToString();
    }
}
