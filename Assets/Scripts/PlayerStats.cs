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
    private int _chargeMax = 100;
    private int _charge = 0;
    public int Charge => _charge;

    public UnityAction ChargeUpEvent;
    public UnityAction ChargeCompleteEvent;
    public UnityAction ChargeResetEvent;

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

    public void ResetCharge()
    {
        _charge = 0;
        ChargeResetEvent?.Invoke();
    }
}
