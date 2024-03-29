﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _comboCounterValue;
    [SerializeField] private TMP_Text _totalComboValue;
    [SerializeField] private Score _score;
    [SerializeField] private Transform _showComboPosition;
    [SerializeField] private Transform _hideComboPosition;
    [SerializeField] private float _timeToShowCombo;
    [SerializeField] private float _timeToHideCombo;
    [SerializeField] private GameObject _comboDisplayGroup;
    [SerializeField] private TMP_Text _comboAdd;
    [SerializeField] private float _addScoreScale = 1.2f;
    [SerializeField] private float _addScoreDelay = .2f;
    [SerializeField] private float _comboAddFadeDelay = 1.5f;
    [SerializeField] private TMP_Text _totalScoreValue;
    [SerializeField] private List<HighComboMessage> _highComboMessages = new List<HighComboMessage>();
    [SerializeField] private GameObject _highComboMsgGroup;
    [SerializeField] private TMP_Text _highComboMsg;
    [SerializeField] private Transform _showMsgPos;
    [SerializeField] private Transform _hideMsgPos;

    private int _moveTweenId;

    private void Start()
    {
        HideComboCounter();
        _totalComboValue.text = "0";
        _comboAdd.alpha = 0;
    }

    public void ShowComboCounter()
    {
        if (LeanTween.descr(_moveTweenId) != null && LeanTween.isTweening(_moveTweenId)) LeanTween.cancel(_moveTweenId);
        _moveTweenId = LeanTween.move(_comboDisplayGroup, _showComboPosition, _timeToShowCombo).setEase(LeanTweenType.easeOutCirc).setIgnoreTimeScale(true).id;
    }

    public void CancelCombo()
    {
        _comboCounterValue.color = Color.red;
        _comboCounterValue.fontStyle = FontStyles.Strikethrough;

        HideComboCounter();
    }

    public void HideComboCounter()
    {
        if (LeanTween.descr(_moveTweenId) != null && LeanTween.isTweening(_moveTweenId)) LeanTween.cancel(_moveTweenId);
        _moveTweenId = LeanTween.move(_comboDisplayGroup, _hideComboPosition, _timeToHideCombo).setEase(LeanTweenType.easeInCirc).setIgnoreTimeScale(true).id;
    }

    public void UpdateComboCounterDisplay()
    {
        _comboCounterValue.text = $"{_score.ComboCounter}";
    }

    public void UpdateTotalComboDisplay(int valueToAdd)
    {
        if (valueToAdd < _score.MinComboToStartCount) return;

        var newTotal = _score.ComboTotalScore + valueToAdd;
        _comboAdd.text = $"+{valueToAdd}";

        LeanTween.value(_comboAdd.gameObject, UpdateAlpha, 1f, 0f, 1f).setEase(LeanTweenType.easeInCirc);
        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.scale(_totalComboValue.GetComponent<RectTransform>(), new Vector3(_addScoreScale, _addScoreScale, 0), _addScoreDelay)
        );
        seq.append(
            LeanTween.scale(_totalComboValue.GetComponent<RectTransform>(), new Vector3(1, 1, 0), _addScoreDelay)
        );
        _totalComboValue.text = newTotal.ToString();
    }

    void UpdateAlpha(float value)
    {
        _comboAdd.alpha = value;
    }

    public void UpdateTotalScoreDisplay()
    {
        _totalScoreValue.text = _score.TotalScore.ToString();
    }

    public void ShowHighComboMessage(int combo)
    {
        if (combo < _score.MinComboToStartCount) return;

        var msg = GetMessageForCombo(combo);
        
        if (msg == "") return;
        
        _highComboMsg.text = msg;

        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.move(_highComboMsgGroup, _showMsgPos, _timeToShowCombo).setEase(LeanTweenType.easeOutCirc).setIgnoreTimeScale(true)
        );
        seq.append(
            LeanTween.delayedCall(1f, HideHighComboMessage)
        );
    }

    public void HideHighComboMessage()
    {
        LeanTween.move(_highComboMsgGroup, _hideMsgPos, _timeToShowCombo).setEase(LeanTweenType.easeInCirc).setIgnoreTimeScale(true);
    }

    private string GetMessageForCombo(int combo)
    {
        for(var i=0; i<_highComboMessages.Count; i++)
        {
            if (combo >= _highComboMessages[i].MinCombo && (i+1 == _highComboMessages.Count || combo < _highComboMessages[i + 1].MinCombo))
            {
                return _highComboMessages[i].Message;
            }
        }
        return "";
    }
}
