using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _xAxis;
    private float _xLimit = 8.40f;

    private Rigidbody2D _rigidbody;

    [SerializeField] private PlayerStats _playerStats;

    IEnumerator _bulletTime;

    private void Awake()
    {
        _bulletTime = BulletTime();    
    }

    void Start()
    {
        _playerStats.Init();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _xAxis = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(_bulletTime);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(_bulletTime);
            Time.timeScale = 1f;
            _bulletTime = BulletTime();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (_playerStats.IsPaddleCharged) return;
            _playerStats.CicleSpecial();
        }

        if (Input.GetButtonDown("Fire3"))
        {
            if (!_playerStats.CanUseSpecial) return;
            _playerStats.ResetCharge();
            if (_playerStats.Special.ChargesPaddle)
            {
                _playerStats.ChargePaddle();
                return;
            }
            ExecuteSpecial();
        }

        if (Input.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    private void FixedUpdate()
    {
        var nextPos = transform.position + (Vector3.right * _xAxis * _speed * Time.fixedUnscaledDeltaTime);
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
        while (_playerStats.CanBulletTime)
        {
            Time.timeScale = _playerStats.BulletTimeScale;
            _playerStats.ConsumeCharge();
            yield return new WaitForSeconds(_playerStats.BulletTimeConsumeRateInSeconds);
        }
        Time.timeScale = 1f;
    }

    public void ActivateSpecial(BallMovement ball)
    {
        if (!_playerStats.IsPaddleCharged) return;
        _playerStats.Special.BallActivatedAction(ball);
    }

    private void ExecuteSpecial()
    {
        _playerStats.Special.Action();
    }
}
