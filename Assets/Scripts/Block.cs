using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private int _hitPoints = 1;
    private Collider2D _collider;
    [SerializeField] private BlockSprites _blockSprites;
    private SpriteRenderer _sprite;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            var normal = (new Vector2(collision.transform.position.x, collision.transform.position.y) - _collider.ClosestPoint(collision.transform.position)).normalized;

            Hit();
            if (_hitPoints <= 0)
            {
                var ball = collision.gameObject.GetComponent<BallMovement>();
                RemoveBlock();
                if (ball.PiercingCountLeft > 0)
                {
                    ball.PiercingCountLeft--;
                    return;
                }
                ball.BounceBall(normal);
            }
            else
            {
                collision.gameObject.GetComponent<BallMovement>().BounceBall(normal);
                UpdateSprite();
            }
        }
    }

    private void Hit()
    {
        _hitPoints -= _playerStats.BallPower;
        if (_hitPoints <= 0)
        {
            RemoveBlock();
        }
        else
        {
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        if (_hitPoints > _blockSprites.Sprites.Length) return;
        _sprite.sprite = _blockSprites.Sprites[_hitPoints - 1];
    }

    private void RemoveBlock()
    {
        _playerStats.ChargeUp();
        Destroy(gameObject);
    }
}
