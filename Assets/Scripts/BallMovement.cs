﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] float _speed;
    [SerializeField] private PlayerStats _playerStats;

    public int PiercingCountLeft = 0;
    public bool IsExplosionActivated = false;
    [SerializeField] private List<GameObject> _explosionsPrefabs;
    [SerializeField] private UpgradeProgress _explosionProgress;

    private Vector2 _previousVelocity;
    private Vector3 _previousPosition;

    [SerializeField] private GameObject _normalBallTrail;
    private GameObject _trail;

    [SerializeField] private Score _score;

    [SerializeField] private float _maxIdleTime = 3f;
    private float _idleTime;

    private Material _ballMaterial;
    [SerializeField] private string _dissolveLayer;
    [SerializeField] private ParticleSystem _aura;


    public event Action OnDestroy;

    void Start()
    {
        _ballMaterial = GetComponent<SpriteRenderer>().material;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.isKinematic = true;
        _aura.Play();
        ChangeTrailToNormal();
    }

    private void Update()
    {
        if (_idleTime > _maxIdleTime)
        {
            _idleTime = 0;
            Dissolve();
        }
        else
        {
            _idleTime += Time.deltaTime;
        }
    }


    private void FixedUpdate()
    {
        _previousVelocity = _rigidbody.velocity;
        _previousPosition = transform.position;
    }
    private void Dissolve()
    {
        gameObject.layer = LayerMask.NameToLayer(_dissolveLayer);
        var trail = _trail.GetComponentInChildren<TrailRenderer>();
        if (trail != null) trail.emitting = false;
        var particles = _trail.GetComponentInChildren<ParticleSystem>();
        if (particles != null) particles.Stop();

        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.value(gameObject, UpdateDissolve, 1f, 0f, 1f)
        );
        seq.append( () =>
        {
            GameObject.FindObjectOfType<PlayerController>()?.BallSpawn();
            Destroy(gameObject);

        }
           
        );
    }

    private void UpdateDissolve(float value)
    {
        _ballMaterial.SetFloat("_Fade", value);
    }


    public void BounceBall(Vector2 inNormal)
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(inNormal.x, inNormal.y, transform.position.z) * 5f, Color.red, 2f);
        _rigidbody.velocity = Vector2.Reflect(_rigidbody.velocity, inNormal);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            _idleTime = 0;
            var playerController = collision.gameObject.GetComponent<PlayerController>();
            if (transform.position.y >= playerController.GetComponent<BoxCollider2D>().bounds.center.y)
            {
                _rigidbody.velocity = playerController.ArrowDirection * _speed;

            }
            playerController.ActivateSpecial(this);
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            DestroyBall();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            _idleTime = 0;
            var block = collision.gameObject.GetComponent<Block>();
            if (PiercingCountLeft > 0)
            {
                _score.ScoreInstantRemove(block);
                PiercingCountLeft--;
                block.RemoveBlock();
                _rigidbody.velocity = _previousVelocity;
                transform.position = _previousPosition;
                if (PiercingCountLeft <= 0) ChangeTrailToNormal();
                return;
            }
            if (IsExplosionActivated)
            {
                Explode();
                ChangeTrailToNormal();
                return;
            }
            block.Hit();
        }
    }


    private void ChangeTrailToNormal()
    {
        if (_trail != null) Destroy(_trail.gameObject);
        _trail = Instantiate(_normalBallTrail, transform.position, Quaternion.identity, transform);
    }

    public void ChangeTrailToSpecial()
    {
        if (_trail != null) Destroy(_trail.gameObject);
        _trail = Instantiate(_playerStats.Special.BallTrail, transform.position, Quaternion.identity, transform);
    }

    private void DestroyBall()
    {
        OnDestroy?.Invoke();
        Destroy(gameObject);
    }

    public void Explode()
    {
        IsExplosionActivated = false;
        Instantiate(_explosionsPrefabs[_explosionProgress.Level], transform.position, Quaternion.identity);
        /*Collider2D[] blockList = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y), 
            _playerStats.ExplosionRadius,
            LayerMask.GetMask("Block")
        );
        foreach (var blockCollider in blockList)
        {
            Block block = blockCollider.GetComponent<Block>();
            _score.ScoreInstantRemove(block);
            block.RemoveBlock();
        }*/
    }

    public void Launch(Vector2 dir)
    {
        _aura.Stop();
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = dir * _speed;
    }
}
