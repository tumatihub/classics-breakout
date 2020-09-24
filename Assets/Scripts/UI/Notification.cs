using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text _valueDisplay;
    [SerializeField] private Image _notificationImage;
    [SerializeField] private float _shakeDelay = 1f;
    [SerializeField] private float _shakeSpeed = .2f;
    [SerializeField] private float _shakeAmplitude = 6f;

    private void Awake()
    {
        _notificationImage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InvokeRepeating("Shake", _shakeDelay, _shakeDelay);
    }

    private void Shake()
    {
        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.rotate(gameObject, new Vector3(0, 0, -_shakeAmplitude), _shakeSpeed)
        );
        seq.append(
            LeanTween.rotate(gameObject, new Vector3(0, 0, _shakeAmplitude), _shakeSpeed)
        );
        seq.append(
            LeanTween.rotate(gameObject, new Vector3(0, 0, 0), _shakeSpeed)
        );
    }

    public void SetValue(int value)
    {
        _valueDisplay.text = value.ToString();
        var IsVisible = true ? value > 0 : false;
        _notificationImage.gameObject.SetActive(IsVisible);
    }
}
