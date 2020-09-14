using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHitDisplay : MonoBehaviour
{
    [SerializeField] private float _moveDist;
    [SerializeField] private float _moveDelay;
    [SerializeField] private float _fadeAlpha;
    private Color _color;
    private TMP_Text _text;

    void Start()
    {
        _text = GetComponent<TMP_Text>();
        _color = _text.color;

        LeanTween.value(gameObject, UpdateAlpha, _color.a, _fadeAlpha, _moveDelay).setEase(LeanTweenType.easeOutCirc);

        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.move(gameObject, transform.position + (Vector3.up * _moveDist), _moveDelay).setEase(LeanTweenType.easeOutCirc)
        );
        seq.append(() => { Destroy(gameObject); });
    }

    void UpdateAlpha(float value)
    {
        _color.a = value;
        _text.color = _color;
    }
}
