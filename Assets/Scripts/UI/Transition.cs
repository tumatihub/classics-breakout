using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Transition : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _startPos;
    [SerializeField] private GameObject _endPos;
    [SerializeField] private GameObject _centerPos;

    [SerializeField] private bool _hasEntrance = true;
    [SerializeField] private bool _hasExit = true;

    [SerializeField] private float _moveDelay = 1f;

    public UnityEvent OnFinishedEntrance;
    public UnityEvent OnFinishedExit;

    void Start()
    {
        if (_hasEntrance)
        {
            RunEntrance();
        }
    }

    public void RunEntrance()
    {
        RunTransition(_centerPos, _endPos, LeanTweenType.easeInCirc, _moveDelay, OnFinishedEntrance);
    }

    public void RunExit()
    {
        RunTransition(_startPos, _centerPos, LeanTweenType.easeOutCirc, _moveDelay, OnFinishedExit);
    }

    public void RunExit(UnityAction callback)
    {
        OnFinishedExit.AddListener(callback);
        RunTransition(_startPos, _centerPos, LeanTweenType.easeOutCirc, _moveDelay, OnFinishedExit);
    }

    private void RunTransition(GameObject start, GameObject end, LeanTweenType easeMode, float time, UnityEvent callbackEvent)
    {
        _image.enabled = true;
        _image.transform.position = start.transform.position;
        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.move(_image.gameObject, end.transform.position, time).setEase(easeMode).setIgnoreTimeScale(true)
        );
        seq.append( () =>
        {
            callbackEvent.Invoke();
        }
        );
    }
}
