using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParticles : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Material _particlesMaterial;
    [SerializeField] private bool _useRandomColors;
    [ColorUsageAttribute(true, true)]
    [SerializeField] private List<Color> _colors = new List<Color>();
    [SerializeField] private float _randomChangeInterval = 3f;

    private void Awake()
    {
        _playerStats.ChangeSpecialEvent += ChangeColorToCurrentSpecial;
        _playerStats.ChargePaddleEvent += ChangeToActivatedColor;
        _playerStats.UnchargePaddleEvent += ChangeColorToCurrentSpecial;
    }

    private void Start()
    {
        if (_useRandomColors && _colors.Count > 0)
        {
            InvokeRepeating("ChangeToRandomColor", 0f, _randomChangeInterval);
        }
        else
        {
            ChangeColorToCurrentSpecial();
        }
    }

    void ChangeToRandomColor()
    {
        var index = Random.Range(0, _colors.Count);
        _particlesMaterial.color = _colors[index];
        _particlesMaterial.SetColor("_EmissionColor", _colors[index]);
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
