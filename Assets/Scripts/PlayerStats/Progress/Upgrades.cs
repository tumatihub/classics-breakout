using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Upgrades : ScriptableObject
{
    [SerializeField] private PlayerStats _playerStats;

    [SerializeField] private InitValues _initValues;

    [SerializeField] private UpgradeProgress _ballPower;
    [SerializeField] private UpgradeProgress _piercingCount;
    [SerializeField] private UpgradeProgress _explosionRadius;
    [SerializeField] private UpgradeProgress _extraCharges;
    [SerializeField] private UpgradeProgress _bulletTimeCost;
    [SerializeField] private UpgradeProgress _barrier;

    [SerializeField] private PiercingSpecial _piercingSpecial;
    [SerializeField] private ExplosionSpecial _explosionSpecial;
    [SerializeField] private BarrierSpecial _barrierSpecial;
    [SerializeField] private EmptySpecial _emptySpecial;
    

    [SerializeField] private List<UpgradeProgress> _allUpgrades = new List<UpgradeProgress>();

    [SerializeField] private Save _save;
    public List<UpgradeProgress> AllUpgrades => _allUpgrades;

    public void LoadUpgrades()
    {
        _save.LoadPlayerPrefs();

        _initValues.BallPower.Value = _ballPower.Value;

        _initValues.ExtraCharges.Value = _extraCharges.Value;

        _initValues.ChargeAmmountConsume.Value = _bulletTimeCost.Value;

        _playerStats.ClearSpecials();

        _playerStats.AddSpecial(_emptySpecial);

        if (_explosionRadius.Level > 0)
        {
            _playerStats.AddSpecial(_explosionSpecial);
        }
        _initValues.ExplosionRadius.Value = _explosionRadius.Value;

        if (_piercingCount.Level > 0)
        {
            _playerStats.AddSpecial(_piercingSpecial);
        }
        _initValues.PiercingCount.Value = _piercingCount.Value;

        if (_barrier.Level > 0)
        {
            _playerStats.AddSpecial(_barrierSpecial);
        }

    }

    public void ResetUpgrades()
    {
        _ballPower.Reset();
        _piercingCount.Reset();
        _explosionRadius.Reset();
        _extraCharges.Reset();
        _bulletTimeCost.Reset();
        _barrier.Reset();
        _save.SavePlayerPrefs();
    }

    public void MaxUpgrades()
    {
        _ballPower.MaxLevel();
        _piercingCount.MaxLevel();
        _explosionRadius.MaxLevel();
        _extraCharges.MaxLevel();
        _bulletTimeCost.MaxLevel();
        _barrier.MaxLevel();
        _save.SavePlayerPrefs();
    }


}
