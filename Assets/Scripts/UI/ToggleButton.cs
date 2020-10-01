using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private Button _button;
    [SerializeField] private BoolVariable _isStateOn;

    public UnityEvent ToggleOnEvent;
    public UnityEvent ToggleOffEvent;

    void Start()
    {
        _button.image.sprite = (_isStateOn.Value) ? _onSprite : _offSprite;
    }

    public void Toggle()
    {
        if (_isStateOn.Value)
        {
            ToggleOffEvent?.Invoke();
            _button.image.sprite = _offSprite;
        }
        else
        {
            ToggleOnEvent?.Invoke();
            _button.image.sprite = _onSprite;
        }
    }
}
