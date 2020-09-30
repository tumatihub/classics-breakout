using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class SelfShake : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _delay;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Start()
    {
        Invoke("Shake", _delay);    
    }

    private void Shake()
    {
        _impulseSource.GenerateImpulse(_force);
    }
}
