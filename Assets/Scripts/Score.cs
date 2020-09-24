using System;
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

    private int _totalScore;
    public int TotalScore => _totalScore;

    public event Action<Block, int> OnHit;

    public void Init()
    {
        _comboTotalScore = 0;
        _comboCounter = 0;
        _totalScore = 0;
    }

    public void ScoreNormalHit(Block block)
    {
        var score = Mathf.Min(block.HitPoints, _playerStats.BallPower);
        _comboCounter += score;
        _totalScore += score;
        OnHit?.Invoke(block, score);
    }

    public void ScoreInstantRemove(Block block)
    {
        var score = block.HitPoints;
        _comboCounter += score;
        _totalScore += score;
        OnHit?.Invoke(block, score);
    }

    public void ScoreComboCouter()
    {
        if (_comboCounter >= _minComboToStartCount)
        {
            _comboTotalScore += _comboCounter;
        }
        _comboCounter = 0;
    }

    public void ConsumeComboPoints(int points)
    {
        _comboTotalScore = Mathf.Max(0, _comboTotalScore - points);
    }
}
