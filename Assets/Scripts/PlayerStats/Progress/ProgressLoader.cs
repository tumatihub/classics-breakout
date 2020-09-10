using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressLoader : MonoBehaviour
{
    [SerializeField] private Upgrades _upgrades;

    void Start()
    {
        _upgrades.LoadUpgrades();    
    }
}
