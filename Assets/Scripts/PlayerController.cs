﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class InputKeys
{
    public const string HORIZONTAL_AXIS = "Horizontal";
    public const string BULLET_TIME = "BulletTime";
    public const string CICLE_SPECIAL = "Cicle";
    public const string SPECIAL = "Special";
    public const string RESTART = "Restart";
    public const string LAUNCH_BALL = "Launch";
    public const KeyCode DEBUG_PANEL = KeyCode.P;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _xAxis;
    private float _xLimit = 8.40f;

    private Rigidbody2D _rigidbody;

    [SerializeField] private PlayerStats _playerStats;

    private Vector2 _arrowDirection = new Vector2(0,1);
    public Vector2 ArrowDirection => _arrowDirection;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _maxArrowDegree = 60f;

    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private BallMovement _ballPrefab;
    private BallMovement _ball;

    private GameObject _chargeParticles;

    IEnumerator _bulletTime;

    private SceneController _sceneController;

    [SerializeField] private ParticleSystem _bulletTimeParticles;
    [SerializeField] private TrailRenderer _bulletTimeTrail;

    [SerializeField] private VolumeProfile _bulletTimeProfile;
    [SerializeField] private float _chromaticValue = .17f;
    [SerializeField] private float _chromaticDelay = .5f;
    private ChromaticAberration _chromatic;

    private PaddleChargeVFX _paddleChargeVFX;

    public UnityEvent BallCollisionWithPaddle;
    public UnityEvent OnBallCollisionWithoutSpecial;

    private void Awake()
    {
        _bulletTime = BulletTime();    
    }

    void Start()
    {
        _playerStats.Init();
        _rigidbody = GetComponent<Rigidbody2D>();
        _sceneController = FindObjectOfType<SceneController>();
        BallSpawn();
        _bulletTimeProfile.TryGet<ChromaticAberration>(out _chromatic);
    }

    void Update()
    {
        _xAxis = Input.GetAxis(InputKeys.HORIZONTAL_AXIS);

        if (Input.GetButtonDown(InputKeys.BULLET_TIME))
        {
            StartCoroutine(_bulletTime);
        }

        if (Input.GetButtonUp(InputKeys.BULLET_TIME))
        {
            StopCoroutine(_bulletTime);
            Time.timeScale = 1f;
            StopBulletTrail();
            StopBulletVolume();
            _bulletTime = BulletTime();
        }

        if (Input.GetButtonDown(InputKeys.CICLE_SPECIAL))
        {
            if (_playerStats.IsPaddleCharged) return;
            _playerStats.CicleSpecial();
            ChangePaddleChargeVFX();
        }

        if (Input.GetButtonDown(InputKeys.SPECIAL))
        {
            if (!_playerStats.CanUseSpecial) return;
            _playerStats.ResetCharge();
            if (_playerStats.Special.ChargesPaddle)
            {
                _playerStats.ChargePaddle();
                ActivateChargeParticles();
                return;
            }
            ExecuteSpecial();
        }

        if (Input.GetButtonDown(InputKeys.RESTART))
        {
            _sceneController.StartGame();
        }

        if (Input.GetButtonDown(InputKeys.LAUNCH_BALL) && _ball != null)
        {
            LauchBall();
        }

        UpdateArrowDirection();

    }

    private void ChangePaddleChargeVFX()
    {
        if (_paddleChargeVFX != null) Destroy(_paddleChargeVFX.gameObject);
        _paddleChargeVFX = Instantiate(_playerStats.Special.PaddleChargeVFX, transform.position, Quaternion.identity, transform);
    }

    private void ActivateChargeParticles()
    {
        _paddleChargeVFX.PulseIn();
    }

    private void LauchBall()
    {
        _ball.transform.parent = null;
        _ball.Launch(_arrowDirection);
        _ball = null;
    }

    private void FixedUpdate()
    {
        var nextPos = transform.position + (Vector3.right * _xAxis * _speed * Time.unscaledDeltaTime);
        if (nextPos.x >= -_xLimit && nextPos.x <= _xLimit)
        {
            _rigidbody.MovePosition(nextPos);
        }
        else
        {
            _rigidbody.MovePosition(new Vector2(_xLimit*(Mathf.Sign(_xAxis)), nextPos.y));
        }
    }

    IEnumerator BulletTime()
    {
        if (_playerStats.CanBulletTime)
        {
            StartBulletTrail();
            StartBulletVolume();
        }
        while (_playerStats.CanBulletTime)
        {
            Time.timeScale = _playerStats.BulletTimeScale;
            _playerStats.ConsumeCharge();
            yield return new WaitForSeconds(_playerStats.BulletTimeConsumeRateInSeconds);
        }
        StopBulletTrail();
        StopBulletVolume();
        Time.timeScale = 1f;
    }

    void StartBulletTrail()
    {
        var particles = _bulletTimeParticles.main;
        particles.loop = true;
        _bulletTimeParticles.Play();
        _bulletTimeTrail.enabled = true;
    }

    void StopBulletTrail()
    {
        var particles = _bulletTimeParticles.main;
        particles.loop = false;
        _bulletTimeTrail.enabled = false;
    }

    void StartBulletVolume()
    {
        LeanTween.value(gameObject, UpdateBulletVolume, 0, _chromaticValue, _chromaticDelay).setEase(LeanTweenType.easeInCirc);
    }

    void StopBulletVolume()
    {
        var currentValue = _chromatic.intensity.value;
        LeanTween.value(gameObject, UpdateBulletVolume, currentValue, 0, _chromaticDelay).setEase(LeanTweenType.easeOutCirc);
    }

    void UpdateBulletVolume(float value)
    {
        _chromatic.intensity.value = value;
    }

    private void UpdateArrowDirection()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newArrowDir = (mousePos - transform.position);
        var angle = Mathf.Atan2(newArrowDir.y, newArrowDir.x)* Mathf.Rad2Deg;
        if (angle >= 90f - _maxArrowDegree && angle <= 90f + _maxArrowDegree)
        {
            _arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            _arrowDirection = newArrowDir.normalized;
        }
    }

    public void ActivateSpecial(BallMovement ball)
    {
        BallCollisionWithPaddle.Invoke();
        if (!_playerStats.IsPaddleCharged)
        {
            OnBallCollisionWithoutSpecial.Invoke();
            return;
        }
        ball.ChangeTrailToSpecial();
        ActivateSpecialParticles();
        _playerStats.Special.BallActivatedAction(ball);
    }

    private void ActivateSpecialParticles()
    {
        _paddleChargeVFX.PulseOut();
    }

    private void ExecuteSpecial()
    {
        _playerStats.Special.Action();
    }

    private void BallSpawn()
    {
        _ball = Instantiate(_ballPrefab, _ballSpawnPoint.position, Quaternion.identity, _ballSpawnPoint);
        _ball.OnDestroy += HandleBallDestroyed;
    }

    private void HandleBallDestroyed()
    {
        Invoke("EndGame", 2f);
    }

    private void EndGame()
    {
        _sceneController.LoadUpgradesScreen();

    }
}
