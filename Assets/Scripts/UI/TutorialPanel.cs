using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] private Button _closeButton;

    private Action _closeCallback;

    public void SetCloseCallback(Action callback)
    {
        _closeCallback = callback;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, 5f);
        _closeCallback?.Invoke();
    }
}
