using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Score _score;
    [SerializeField] private ScoreDisplay _scoreDisplay;
    private float _comboCooldown;
    [SerializeField] private TMP_Text _scoreHitDisplayPrefab;
    [SerializeField] private Canvas _scoreHitCanvasPrefab;
    private Canvas _scoreHitCanvas;

    private void Awake()
    {
        _score.Init();
        _score.OnHit += HandleHit;
    }

    private void Start()
    {
        _scoreHitCanvas = Instantiate(_scoreHitCanvasPrefab);    
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
        _scoreDisplay.UpdateTotalComboDisplay(_score.ComboCounter);
        _score.ScoreComboCouter();
        _scoreDisplay.HideComboCounter();
        _comboCooldown = 0;
    }

    void HandleHit(Block block, int score)
    {
        SpawnHitValue(block.transform.position, score);
        if (_score.ComboCounter >= _score.MinComboToStartCount)
        {
            if (_comboCooldown <= 0) _scoreDisplay.ShowComboCounter();
            _comboCooldown = _score.ComboCounterDurationWithoutHit;
            _scoreDisplay.UpdateComboCounterDisplay();
        }
        _scoreDisplay.UpdateTotalScoreDisplay();
    }

    void SpawnHitValue(Vector3 position, int score)
    {
        var hitValue = Instantiate(_scoreHitDisplayPrefab, position, Quaternion.identity, _scoreHitCanvas.transform);
        hitValue.text = score.ToString();
        
    }

    public void CancelCombo()
    {
        _score.CancelCombo();
        _scoreDisplay.CancelCombo();
    }

    private void OnDestroy()
    {
        _score.OnHit -= HandleHit;
    }
}
