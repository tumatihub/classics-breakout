﻿using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private InitValues _initValues;
    private int _ballPower = 3;
    public int BallPower => _ballPower;
    private int _maxBallPower;

    private int _chargeAmmountPerHit = 1;
    private int _chargeAmmountPerRemovedBlock = 5;
    private int _chargeAmmountConsume = 5;
    private float _bulletTimeConsumeRateInSeconds = 1f;
    public float BulletTimeConsumeRateInSeconds => _bulletTimeConsumeRateInSeconds;
    private float _bulletTimeScale = .5f;
    public float BulletTimeScale => _bulletTimeScale;

    private int _chargeMax = 100;
    public int ChargeMax => _chargeMax;
    private int _charge = 0;
    private bool _isPaddleCharged;
    public bool IsPaddleCharged => _isPaddleCharged;
    public int Charge => _charge;

    private int _piercingCount = 0;
    public int PiercingCount => _piercingCount;

    public UnityAction ChargeUpEvent;
    public UnityAction ChargeCompleteEvent;
    public UnityAction ChargeResetEvent;
    public UnityAction ChargeConsumeEvent;
    public UnityAction ChangeSpecialEvent;

    public bool CanBulletTime => _charge >= _chargeAmmountConsume;

    [SerializeField] private Special[] _specials;
    private Special _special;
    public bool CanUseSpecial => _charge >= _chargeMax && _special != null;
    public Special Special => _special;
    private int _specialIndex = 0;

    private float _explosionRadius;
    public float ExplosionRadius => _explosionRadius;

    public void Init()
    {
        ResetCharge();
        _special = null;
        _isPaddleCharged = false;

        _ballPower = (int)_initValues.BallPower.Value;
        _chargeAmmountPerHit = (int)_initValues.ChargeAmmountPerHit.Value;
        _chargeAmmountPerRemovedBlock = (int)_initValues.ChargeAmmountPerRemovedBlock.Value;
        _chargeAmmountConsume = (int)_initValues.ChargeAmmountConsume.Value;
        _bulletTimeConsumeRateInSeconds = _initValues.BulletTimeConsumeRateInSeconds.Value;
        _bulletTimeScale = _initValues.BulletTimeScale.Value;
        _chargeMax = (int)_initValues.ChargeMax.Value;
        _piercingCount = (int)_initValues.PiercingCount.Value;
        _explosionRadius = _initValues.ExplosionRadius.Value;
    }

    public void CicleSpecial()
    {
        if (_specials.Length == 0) return;

        _specialIndex += 1;
        if (_specialIndex >= _specials.Length) _specialIndex = 0;
        _special = _specials[_specialIndex];
        ChangeSpecialEvent?.Invoke();
    }

    public void LevelUpBallPower()
    {
        _ballPower = Mathf.Min(_ballPower + 1, _maxBallPower);
    }

    public void ChargePerHit()
    {
        ChargeUp(_chargeAmmountPerHit);
    }

    public void ChargePerRemovedBlock()
    {
        ChargeUp(_chargeAmmountPerRemovedBlock);
    }
    private void ChargeUp(int ammount)
    {
        _charge = Mathf.Min(_charge + ammount, _chargeMax);
        ChargeUpEvent?.Invoke();
        if (_charge == _chargeMax) ChargeCompleteEvent?.Invoke();
    }

    public void ConsumeCharge()
    {
        _charge = Mathf.Max(_charge - _chargeAmmountConsume, 0);
        ChargeConsumeEvent?.Invoke();
    }

    public void ResetCharge()
    {
        _charge = 0;
        ChargeResetEvent?.Invoke();
    }

    public void ChargePaddle()
    {
        _isPaddleCharged = true;
    }

    public void UnchargePaddle()
    {
        _isPaddleCharged = false;
    }
}