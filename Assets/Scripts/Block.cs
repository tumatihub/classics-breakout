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

    public void Hit()
    {
        _hitPoints -= _playerStats.BallPower;
        if (_hitPoints <= 0)
        {
            RemoveBlock();
        }
        else
        {
            UpdateSprite();
            _playerStats.ChargePerHit();
        }
    }

    private void UpdateSprite()
    {
        if (_hitPoints > _blockSprites.Sprites.Length) return;
        _sprite.sprite = _blockSprites.Sprites[_hitPoints - 1];
    }

    public void RemoveBlock()
    {
        _playerStats.ChargePerRemovedBlock();
        Destroy(gameObject);
    }
}
