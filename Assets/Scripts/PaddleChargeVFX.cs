using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleChargeVFX : MonoBehaviour
{
    private const float START_PULSE_DELAY = .4f;
    [SerializeField] private ParticleSystem _pulseIn;
    [SerializeField] private ParticleSystem _pulseOut;
    [SerializeField] private SpriteRenderer _paddleSprite;
    [SerializeField] private AudioClip _pulseInClip;
    [SerializeField] private AudioClip _pulseClip;
    [SerializeField] private AudioClip _pulseOutClip;
    private AudioSource _audioSource;

    private void Start()
    {
        _paddleSprite.enabled = false;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _pulseClip;
        _audioSource.loop = true;
    }

    public void PulseIn()
    {
        _pulseOut.Stop();
        _paddleSprite.enabled = true;
        _pulseIn.Play();
        _audioSource.PlayOneShot(_pulseInClip);
        StartPulseAudio();
    }

    public void PulseOut()
    {
        _audioSource.Stop();
        _pulseIn.Stop();
        _pulseOut.Play();
        _audioSource.PlayOneShot(_pulseOutClip);
        _paddleSprite.enabled = false;
    }

    private void StartPulseAudio()
    {
        _audioSource.PlayDelayed(START_PULSE_DELAY);
    }
}
