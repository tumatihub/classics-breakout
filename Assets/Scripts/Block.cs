﻿using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    [SerializeField] private PlayerStats _playerStats;
    private int _hitPoints = 1;
    private Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            var normal = (new Vector2(collision.transform.position.x, collision.transform.position.y) - _collider.ClosestPoint(collision.transform.position)).normalized;
            
            _hitPoints -= _playerStats.BallPower;
            if (_hitPoints <= 0)
            {
                collision.gameObject.GetComponent<BallMovement>().BounceBall(normal);
                RemoveBlock();
            }
            else
            {
                collision.gameObject.GetComponent<BallMovement>().BounceBall(normal);
                UpdateSprite();
            }
        }
    }

    private void UpdateSprite()
    {
        throw new NotImplementedException();
    }

    private void RemoveBlock()
    {
        _playerStats.ChargeUp();
        Destroy(gameObject);
    }
}
