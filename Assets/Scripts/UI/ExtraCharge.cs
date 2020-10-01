using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraCharge : MonoBehaviour
{
    [SerializeField] private Image _foreground;
    private bool _isActive;
    public bool IsActive => _isActive;
    [SerializeField] private AudioClip _activateSFX;
    private AudioSource _audioSource;

    Coroutine _fillImage;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        _foreground.fillAmount = 0;
    }

    IEnumerator FillImage(float time)
    {
        float cooldown = 0f;
        float fill = 0f;
        while (cooldown < time)
        {
            cooldown += Time.deltaTime;
            fill = cooldown / time;
            _foreground.fillAmount = fill;
            yield return new WaitForEndOfFrame();
        }
        _isActive = true;
        StoreScaleFeedback();
        _audioSource.PlayOneShot(_activateSFX);
    }

    private void StoreScaleFeedback()
    {
        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.scale(_foreground.GetComponent<RectTransform>(), new Vector3(1.2f, 1.5f, 1f), .1f)
        );
        seq.append(
            LeanTween.scale(_foreground.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), .1f)
        );
    }

    public void StartStoring(float delay)
    {
        _fillImage = StartCoroutine("FillImage", delay);
    }

    public void CancelStoring()
    {
        if (_fillImage != null) StopCoroutine(_fillImage);
        _foreground.fillAmount = 0;
        _isActive = false;
    }

    public void Consume()
    {
        _foreground.fillAmount = 0;
        _isActive = false;
    }

    public void ChangeColor(Color color)
    {
        _foreground.color = color;
    }
}
