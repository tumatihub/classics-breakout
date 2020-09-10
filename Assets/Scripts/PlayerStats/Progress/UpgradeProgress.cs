using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Upgrade
{
    public int Price;
    public float Value;
}

[CreateAssetMenu]
public class UpgradeProgress : ScriptableObject
{
    [SerializeField]
    private int _level = 0;
    public int Level => _level;
    [SerializeField]
    List<Upgrade> _upgrades = new List<Upgrade>();

    public float Value => _upgrades[_level].Value;
    public int Price => _upgrades[_level].Price;

    public float GetNextValue()
    {
        return _upgrades[Mathf.Min(_level + 1, _upgrades.Count - 1)].Value;
    }

    public float GetNextPrice()
    {
        return _upgrades[Mathf.Min(_level + 1, _upgrades.Count - 1)].Price;
    }
}
