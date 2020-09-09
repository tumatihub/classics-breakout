using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Score _score;
    [SerializeField] private ScoreDisplay _scoreDisplay;
    private float _comboCooldown;

    private void Awake()
    {
        _score.Init();
        _score.OnHit += HandleHit;
    }

    void Update()
    {
        if (_comboCooldown > 0)
        {
            _comboCooldown -= Time.deltaTime;
            if (_comboCooldown <= 0) ScoreComboCounter();
        }
    }

    public void ScoreComboCounter()
    {
        _score.ScoreComboCouter();
        _scoreDisplay.HideComboCounter();
        _scoreDisplay.UpdateTotalComboDisplay();
        _comboCooldown = 0;
    }

    void HandleHit()
    {
        if (_score.ComboCounter >= _score.MinComboToStartCount)
        {
            if (_comboCooldown <= 0) _scoreDisplay.ShowComboCounter();
            _comboCooldown = _score.ComboCounterDurationWithoutHit;
            _scoreDisplay.UpdateComboCounterDisplay();
        }
    }
}
