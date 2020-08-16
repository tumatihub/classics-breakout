using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    private int _ballPower = 1;
    public int BallPower => _ballPower;
    private int _maxBallPower;

    [SerializeField] private int _chargeAmmountPerHit = 1;
    [SerializeField] private int _chargeAmmountConsume = 5;
    [SerializeField] private float _bulletTimeConsumeRateInSeconds = 1f;
    public float BulletTimeConsumeRateInSeconds => _bulletTimeConsumeRateInSeconds;
    [SerializeField] private float _bulletTimeScale = .5f;
    public float BulletTimeScale => _bulletTimeScale;

    private int _chargeMax = 100;
    private int _charge = 0;
    public int Charge => _charge;

    public UnityAction ChargeUpEvent;
    public UnityAction ChargeCompleteEvent;
    public UnityAction ChargeResetEvent;
    public UnityAction ChargeConsumeEvent;

    public bool CanBulletTime =>_charge >= _chargeAmmountConsume;
    public void Init()
    {
        ResetCharge();
    }

    public void LevelUpBallPower()
    {
        _ballPower = Mathf.Min(_ballPower + 1, _maxBallPower);
    }

    public void ChargeUp()
    {
        _charge = Mathf.Min(_charge + _chargeAmmountPerHit, _chargeMax);
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
}
