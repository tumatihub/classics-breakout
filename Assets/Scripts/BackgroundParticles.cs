using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticles : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Material _particlesMaterial;

    private void Awake()
    {
        _playerStats.ChangeSpecialEvent += ChangeColorToCurrentSpecial;
        _playerStats.ChargePaddleEvent += ChangeToActivatedColor;
        _playerStats.UnchargePaddleEvent += ChangeColorToCurrentSpecial;
    }

    void ChangeColorToCurrentSpecial()
    {
        _particlesMaterial.color = _playerStats.Special.BackgroundColor;
        _particlesMaterial.SetColor("_EmissionColor", _playerStats.Special.BackgroundHDRColor);
    }

    void ChangeToActivatedColor()
    {
        _particlesMaterial.SetColor("_EmissionColor", _playerStats.Special.BackgroundHDRColorActivated);
    }

    private void OnDestroy()
    {
        _playerStats.ChangeSpecialEvent -= ChangeColorToCurrentSpecial;
        _playerStats.ChargePaddleEvent -= ChangeToActivatedColor;
        _playerStats.UnchargePaddleEvent -= ChangeColorToCurrentSpecial;
    }
}
