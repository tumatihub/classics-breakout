using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputKeys
{
    public const string HORIZONTAL_AXIS = "Horizontal";
    public const string BULLET_TIME = "BulletTime";
    public const string CICLE_SPECIAL = "Cicle";
    public const string SPECIAL = "Special";
    public const string RESTART = "Restart";
    public const string LAUNCH_BALL = "Launch";
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private float _xAxis;
    private float _xLimit = 8.40f;

    private Rigidbody2D _rigidbody;

    [SerializeField] private PlayerStats _playerStats;

    private Vector2 _arrowDirection;
    public Vector2 ArrowDirection => _arrowDirection;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _maxArrowDegree = 60f;

    [SerializeField] private Transform _ballSpawnPoint;
    [SerializeField] private BallMovement _ballPrefab;
    private BallMovement _ball;


    IEnumerator _bulletTime;

    private void Awake()
    {
        _bulletTime = BulletTime();    
    }

    void Start()
    {
        _playerStats.Init();
        _rigidbody = GetComponent<Rigidbody2D>();
        BallSpawn();
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
            _bulletTime = BulletTime();
        }

        if (Input.GetButtonDown(InputKeys.CICLE_SPECIAL))
        {
            if (_playerStats.IsPaddleCharged) return;
            _playerStats.CicleSpecial();
        }

        if (Input.GetButtonDown(InputKeys.SPECIAL))
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

        if (Input.GetButtonDown(InputKeys.RESTART))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetButtonDown(InputKeys.LAUNCH_BALL) && _ball != null)
        {
            LauchBall();
        }

        UpdateArrowDirection();

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
        while (_playerStats.CanBulletTime)
        {
            Time.timeScale = _playerStats.BulletTimeScale;
            _playerStats.ConsumeCharge();
            yield return new WaitForSeconds(_playerStats.BulletTimeConsumeRateInSeconds);
        }
        Time.timeScale = 1f;
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
        if (!_playerStats.IsPaddleCharged) return;
        _playerStats.Special.BallActivatedAction(ball);
    }

    private void ExecuteSpecial()
    {
        _playerStats.Special.Action();
    }

    private void BallSpawn()
    {
        _ball = Instantiate(_ballPrefab, _ballSpawnPoint.position, Quaternion.identity, _ballSpawnPoint);
    }
}
