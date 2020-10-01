using System;
using System.Collections.Generic;
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
    public UnityAction ChargePaddleEvent;
    public UnityAction UnchargePaddleEvent;
    public UnityAction OnStoreExtraCharge;
    public UnityAction<int> OnConsumeExtraCharge;
    public UnityAction<float> OnStartStoringExtraCharge;
    public UnityAction OnCancelStoringExtraCharge;

    public bool CanBulletTime => _charge >= _chargeAmmountConsume;

    [SerializeField] private List<Special> _specials = new List<Special>();
    public List<Special> Specials => _specials;

    private Special _special;

    public bool IsFullCharged => _charge >= _chargeMax;

    public bool HasStoredCharge => _currentStoredCharges > 0;

    public bool CanStoreCharge => _currentStoredCharges < _extraCharges;
    public Special Special => _special;
    private int _specialIndex = 0;

    private float _explosionRadius;
    public float ExplosionRadius => _explosionRadius;

    private int _extraCharges;
    public int ExtraCharges => _extraCharges;

    private int _currentStoredCharges;

    [SerializeField] private Upgrades _upgrades;

    public void Init()
    {
        _upgrades.LoadUpgrades();
        ResetCharge();
        ChangeSpecial(0);
        _isPaddleCharged = false;
        _currentStoredCharges = 0;

        _ballPower = (int)_initValues.BallPower.Value;
        _chargeAmmountPerHit = (int)_initValues.ChargeAmmountPerHit.Value;
        _chargeAmmountPerRemovedBlock = (int)_initValues.ChargeAmmountPerRemovedBlock.Value;
        _chargeAmmountConsume = (int)_initValues.ChargeAmmountConsume.Value;
        _bulletTimeConsumeRateInSeconds = _initValues.BulletTimeConsumeRateInSeconds.Value;
        _bulletTimeScale = _initValues.BulletTimeScale.Value;
        _chargeMax = (int)_initValues.ChargeMax.Value;
        _piercingCount = (int)_initValues.PiercingCount.Value;
        _explosionRadius = _initValues.ExplosionRadius.Value;
        _extraCharges = (int)_initValues.ExtraCharges.Value;
    }

    public bool CanUseSpecial()
    {
        if (!_special.CanBeUsed) return false;
        var currentCharge = _currentStoredCharges;
        currentCharge += IsFullCharged ? 1 : 0;
        return currentCharge >= _special.ChargeCost;
    }

    public void ClearSpecials()
    {
        _specials.Clear();
    }

    public void AddSpecial(Special special)
    {
        _specials.Add(special);
    }

    public void CycleUpSpecial()
    {
        if (_specials.Count == 0) return;

        _specialIndex += 1;
        if (_specialIndex >= _specials.Count) _specialIndex = 0;
        ChangeSpecial(_specialIndex);
    }

    public void CycleDownSpecial()
    {
        if (_specials.Count == 0) return;

        _specialIndex -= 1;
        if (_specialIndex < 0) _specialIndex = _specials.Count-1;
        ChangeSpecial(_specialIndex);
    }

    public void ChangeSpecial(int index)
    {
        _special = _specials[index];
        _specialIndex = index;
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
        if (_charge < _chargeMax)
        {
            _charge = Mathf.Min(_charge + ammount, _chargeMax);
            ChargeUpEvent?.Invoke();
            if (IsFullCharged) ChargeCompleteEvent?.Invoke();
        }

    }

    public void ConsumeCharge()
    {
        _charge = Mathf.Max(_charge - _chargeAmmountConsume, 0);
        ChargeConsumeEvent?.Invoke();
    }

    
    public void StoreExtraCharge()
    {
        _currentStoredCharges = Mathf.Min(_currentStoredCharges + 1, _extraCharges);
        ResetCharge();
        OnStoreExtraCharge?.Invoke();
    }

    public void ResetCharge()
    {
        _charge = 0;
        ChargeResetEvent?.Invoke();
    }

    public void ConsumeSpecial()
    {
        if (_special.ChargeCost > _currentStoredCharges)
        {
            ResetCharge();
            _currentStoredCharges = Mathf.Max(_currentStoredCharges - _special.ChargeCost-1, 0);
            OnConsumeExtraCharge?.Invoke(_special.ChargeCost-1);
        }
        else
        {
            _currentStoredCharges = Mathf.Max(_currentStoredCharges - _special.ChargeCost, 0);
            OnConsumeExtraCharge?.Invoke(_special.ChargeCost);
        }
    }

    public void ChargePaddle()
    {
        _isPaddleCharged = true;
        ChargePaddleEvent?.Invoke();
    }

    public void UnchargePaddle()
    {
        _isPaddleCharged = false;
        UnchargePaddleEvent?.Invoke();
    }
}
