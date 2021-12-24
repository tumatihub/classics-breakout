using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class ProgressLoader : MonoBehaviour
{
    [SerializeField] private Upgrades _upgrades;

    void Start()
    {
        AnalyticsEvent.GameStart();
        _upgrades.LoadUpgrades();    
    }
}
