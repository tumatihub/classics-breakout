﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Score : ScriptableObject
{
    [SerializeField] PlayerStats _playerStats;
    private int _comboTotalScore;
    public int ComboTotalScore => _comboTotalScore;
    private int _comboCounter;
    public int ComboCounter => _comboCounter;
    private int _minComboToStartCount = 5;
    public int MinComboToStartCount => _minComboToStartCount;
    private float _comboCounterDurationWithoutHit = 3f;
    public float ComboCounterDurationWithoutHit => _comboCounterDurationWithoutHit;

    public event Action OnHit;

    public void Init()
    {
        _comboTotalScore = 0;
        _comboCounter = 0;
    }

    public void ScoreNormalHit(Block block)
    {
        OnHit?.Invoke();
        _comboCounter += Mathf.Min(block.HitPoints, _playerStats.BallPower);
    }

    public void ScoreInstantRemove(Block block)
    {
        OnHit?.Invoke();
        _comboCounter += block.HitPoints;
    }

    public void ScoreComboCouter()
    {
        if (_comboCounter >= _minComboToStartCount)
        {
            _comboTotalScore += _comboCounter;
        }
        _comboCounter = 0;
    }

}