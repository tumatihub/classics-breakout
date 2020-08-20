using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPainel : MonoBehaviour
{
    private bool _hiding = true;

    [SerializeField] private Transform _showLeftPos;
    [SerializeField] private Transform _hideLeftPos;

    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(InputKeys.DEBUG_PANEL))
        {
            if (_hiding)
            {
                ShowPanel();
            }
            else
            {
                HidePanel();
            }
        }    
    }

    void ShowPanel()
    {
        _hiding = false;
        LeanTween.move(gameObject, _showLeftPos, .5f).setEase(LeanTweenType.easeInQuad).setIgnoreTimeScale(true);
        Time.timeScale = 0;
    }

    void HidePanel()
    {
        _hiding = true;
        Time.timeScale = 1;
        LeanTween.move(gameObject, _hideLeftPos, .5f).setEase(LeanTweenType.easeInQuad).setIgnoreTimeScale(true);
    }
}
