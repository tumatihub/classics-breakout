using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleChargeVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _pulseIn;
    [SerializeField] private ParticleSystem _pulseOut;
    [SerializeField] private SpriteRenderer _paddleSprite;

    private void Start()
    {
        _paddleSprite.enabled = false;    
    }

    public void PulseIn()
    {
        _pulseOut.Stop();
        _paddleSprite.enabled = true;
        _pulseIn.Play();
    }

    public void PulseOut()
    {
        _pulseIn.Stop();
        _pulseOut.Play();
        _paddleSprite.enabled = false;
    }
}
