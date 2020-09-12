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

    [SerializeField] private PiercingSpecial _piercingSpecial;
    [SerializeField] private ExplosionSpecial _explosionSpecial;

    public void LoadUpgrades()
    {
        _initValues.BallPower.Value = _ballPower.Value;

        _playerStats.ClearSpecials();
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
    }

    public void ResetUpgrades()
    {
        _ballPower.Reset();
        _piercingCount.Reset();
        _explosionRadius.Reset();
    }
}
