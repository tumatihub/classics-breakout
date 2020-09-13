using System;
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
    [SerializeField] private ExplosionFX _explosionFX;

    private Vector2 _previousVelocity;
    private Vector3 _previousPosition;

    [SerializeField] private GameObject _normalBallTrail;
    private GameObject _trail;

    [SerializeField] private Score _score;


    public event Action OnDestroy;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.isKinematic = true;
        ChangeTrailToNormal();
    }

    private void FixedUpdate()
    {
        _previousVelocity = _rigidbody.velocity;
        _previousPosition = transform.position;
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
        Instantiate(_explosionFX, transform.position, Quaternion.identity);
        Collider2D[] blockList = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y), 
            _playerStats.ExplosionRadius,
            LayerMask.GetMask("Block")
        );
        foreach (var blockCollider in blockList)
        {
            Block block = blockCollider.GetComponent<Block>();
            block.RemoveBlock();
        }
    }

    public void Launch(Vector2 dir)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = dir * _speed;
    }
}
