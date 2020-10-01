using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChargeUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] TMP_Text _specialName;
    [SerializeField] Image _progressBar;
    [SerializeField] Image _centerProgressBar;
    [SerializeField] private Image _icon;
    private float _slideBarMaxWidth;
    [SerializeField] private Transform _specialSelectionGroup;
    [SerializeField] private SpecialOption _specialOptionPrefab;
    [SerializeField] private GameObject _extraChargeGroup;
    [SerializeField] private ExtraCharge _extraChargePrefab;
    private List<ExtraCharge> _extraCharges = new List<ExtraCharge>();
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _fullChargeClip;
    

    private void OnEnable()
    {
        _playerStats.ChargeUpEvent += UpdateChargeValue;
        _playerStats.ChargeConsumeEvent += UpdateChargeValue;
        _playerStats.ChangeSpecialEvent += ChangeSpecial;
        _playerStats.ChargeResetEvent += UpdateChargeValue;
        _playerStats.OnConsumeExtraCharge += HandleConsumeExtraCharge;
        _playerStats.OnStartStoringExtraCharge += HandleStartStoringExtraCharge;
        _playerStats.OnCancelStoringExtraCharge += HandleCancelStoringExtraCharge;
        _playerStats.ChargeCompleteEvent += HandleChargeComplete;
    }

    private void Awake()
    {
        _slideBarMaxWidth = _progressBar.rectTransform.sizeDelta.x;
        _centerProgressBar.material.SetInt("_IsActive", 0);
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _specialSelectionGroup.gameObject.SetActive(false);
        CreateSelectionOptions();
        CreateExtraChargesList();
    }

    private void CreateExtraChargesList()
    {
        for (var i = 0; i<_playerStats.ExtraCharges; i++)
        {
            var charge = Instantiate(_extraChargePrefab, _extraChargeGroup.transform);
            _extraCharges.Add(charge);
        }
    }

    private void CreateSelectionOptions()
    {
        for (var i=_playerStats.Specials.Count-1; i>=0; i--)
        {
            var option = Instantiate(_specialOptionPrefab, _specialSelectionGroup);
            option.SetSpecial(_playerStats.Specials[i]);
            option.UpdateDisplay();
        }
    }

    private void UpdateChargeValue()
    {
        var rect = _progressBar.rectTransform;
        rect.sizeDelta = new Vector2(Mathf.Lerp(0, _slideBarMaxWidth, (float)_playerStats.Charge / (float)_playerStats.ChargeMax), rect.sizeDelta.y);
    }

    private void ChangeSpecial()
    {
        _specialName.text = _playerStats.Special.Name;
        _progressBar.color = _playerStats.Special.Color;
        _centerProgressBar.material.SetColor("_Color", _playerStats.Special.Color);
        foreach (var charge in _extraCharges) charge.ChangeColor(_playerStats.Special.Color);
        _icon.sprite = _playerStats.Special.Icon;
    }

    public void HandleConsumeExtraCharge(int charges)
    {
        int removedCharges = 0;
        for(var i = _extraCharges.Count-1; i>=0; i--)
        {
            if (_extraCharges[i].IsActive)
            {
                _extraCharges[i].Consume();
                removedCharges++;
                if (removedCharges >= charges) return;
            }
        }
    }

    public void HandleEnterBulletTime()
    {
        _centerProgressBar.material.SetInt("_IsActive", 1);
    }

    public void HandleExitBulletTime()
    {
        _centerProgressBar.material.SetInt("_IsActive", 0);
    }

    public void HandleSpecialCycleUp()
    {
        _specialSelectionGroup.GetChild(_playerStats.Specials.Count - 1).SetSiblingIndex(0);
    }

    public void HandleSpecialCycleDown()
    {
        _specialSelectionGroup.GetChild(0).SetSiblingIndex(_playerStats.Specials.Count - 1);
    }

    public void HandleEnterSpecialSelection()
    {
        _specialSelectionGroup.gameObject.SetActive(true);
    }

    public void HandleExitSpecialSelection()
    {
        _specialSelectionGroup.gameObject.SetActive(false);
    }
    private void HandleCancelStoringExtraCharge()
    {
        foreach (var charge in _extraCharges)
        {
            if (!charge.IsActive)
            {
                charge.CancelStoring();
                return;
            }
        }
    }

    private void HandleStartStoringExtraCharge(float timeToStore)
    {
        foreach (var charge in _extraCharges)
        {
            if (!charge.IsActive)
            {
                charge.StartStoring(timeToStore);
                return;
            }
        }
    }

    private void HandleChargeComplete()
    {
        _audioSource.PlayOneShot(_fullChargeClip);
        FullChargeFeedback();
    }
    private void FullChargeFeedback()
    {
        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.scale(_progressBar.GetComponent<RectTransform>(), new Vector3(1f, 2f, 1f), .1f)
        );
        seq.append(
            LeanTween.scale(_progressBar.GetComponent<RectTransform>(), new Vector3(1f, 1f, 1f), .1f)
        );
    }

    private void OnDisable()
    {
        _playerStats.ChargeUpEvent -= UpdateChargeValue;
        _playerStats.ChargeConsumeEvent -= UpdateChargeValue;
        _playerStats.ChangeSpecialEvent -= ChangeSpecial;
        _playerStats.ChargeResetEvent -= UpdateChargeValue;
        _playerStats.OnConsumeExtraCharge -= HandleConsumeExtraCharge;
        _playerStats.OnStartStoringExtraCharge -= HandleStartStoringExtraCharge;
        _playerStats.OnCancelStoringExtraCharge -= HandleCancelStoringExtraCharge;
        _playerStats.ChargeCompleteEvent -= HandleChargeComplete;
    }

}
