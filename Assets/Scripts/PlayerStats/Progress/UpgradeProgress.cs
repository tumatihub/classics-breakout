using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public int Price;
    public float Value;
    public string Description;
}

[CreateAssetMenu]
public class UpgradeProgress : ScriptableObject
{
    [SerializeField]
    private int _level = 0;
    public int Level => _level;
    [SerializeField]
    List<Upgrade> _upgrades = new List<Upgrade>();

    [SerializeField] private string _upgradeName;
    public string UpgradeName => _upgradeName;

    public float Value => _upgrades[_level].Value;
    public int Price => _upgrades[_level].Price;

    public string Description => _upgrades[_level].Description;

    public bool IsMaxed => _level >= _upgrades.Count - 1;
    public float GetNextValue()
    {
        return _upgrades[Mathf.Min(_level + 1, _upgrades.Count - 1)].Value;
    }

    public float GetNextPrice()
    {
        return _upgrades[Mathf.Min(_level + 1, _upgrades.Count - 1)].Price;
    }

    public string GetNextDescription()
    {
        return _upgrades[Mathf.Min(_level + 1, _upgrades.Count - 1)].Description;
    }

    public void LevelUp()
    {
        _level = Mathf.Min(_upgrades.Count - 1, _level + 1);
    }

    public void Reset()
    {
        _level = 0;
    }
}
