﻿using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBlock : MonoBehaviour
{
    [SerializeField] private int _hitPoints = 1;
    public int HitPoints => _hitPoints;

    private Collider2D _collider;
    [SerializeField] private BlockTypes _blockTypes;
    private SpriteRenderer _sprite;

    /*[SerializeField] private Score _score;*/
    /*[SerializeField] private GameObject _destroyParticles;*/

    private int _ballPower = 1;
    private int _originalHitPoints;

    private void Awake()
    {
        _originalHitPoints = _hitPoints;
    }

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        UpdateSprite();
        Create();
    }

    private void Create()
    {
        var seq = LeanTween.sequence();
        seq.append(LeanTween.value(gameObject, UpdateDissolveFadeValue, 0f, 1f, .5f));
    }

    public void Hit()
    {
        if (_hitPoints -_ballPower <= 0)
        {
            RemoveBlock();
        }
        else
        {
            _hitPoints -= _ballPower;
            DissolveToOtherSprite();
            SpawnHitParticles();
        }
    }

    /*public void SetHitPoints(int hitPoints)
    {
        _hitPoints = Mathf.Clamp(hitPoints, 1, _blockTypes.Configs.Length);
    }*/

    private void UpdateSprite()
    {
        if (_hitPoints > _blockTypes.Configs.Length || _hitPoints < 1) return;
        _sprite.sprite = _blockTypes.Configs[_hitPoints - 1].Sprite;
        _sprite.material.SetColor("_Color", _blockTypes.Configs[_hitPoints - 1].DissolveColor);
        _sprite.material.SetFloat("_Offset", UnityEngine.Random.Range(0f, 5f));
    }

    public void RemoveBlock()
    {
        Instantiate(_blockTypes.Configs[_hitPoints - 1].DestroyParticles, transform.position, Quaternion.identity);
        _collider.enabled = false;
        Dissolve();
        Invoke("Respawn", 1.5f);
    }

    public void Dissolve()
    {
        var seq = LeanTween.sequence();
        seq.append(LeanTween.value(gameObject, UpdateDissolveFadeValue, 1f, 0f, .3f));
        seq.append(() => { /*OnRemoveBlock?.Invoke(); Destroy(gameObject);*/ });
    }

    public void Respawn()
    {
        _collider.enabled = true;
        _hitPoints = _originalHitPoints;
        UpdateSprite();
        Create();
    }

    public Vector2 GetNormal(Vector3 position)
    {
        if (position.x >= _collider.bounds.min.x && position.x <= _collider.bounds.max.x)
        {
            if (position.y >= _collider.bounds.center.y)
            {
                return new Vector2(0f, 1f);
            }
            else
            {
                return new Vector2(0f, -1f);
            }
        }
        else
        {
            if (position.x >= _collider.bounds.center.x)
            {
                return new Vector2(1f, 0f);
            }
            else
            {
                return new Vector2(-1f, 0f);
            }
        }
    }

    void DissolveToOtherSprite()
    {
        var seq = LeanTween.sequence();
        seq.append(LeanTween.value(gameObject, UpdateDissolveFadeValue, 1f, 0.2f, .3f));
        seq.append(UpdateSprite);
        seq.append(LeanTween.value(gameObject, UpdateDissolveFadeValue, 0.2f, 1f, .3f));
    }

    void UpdateDissolveFadeValue(float value, float ratio)
    {
        _sprite.material.SetFloat("_Fade", value);
    }

    void SpawnHitParticles()
    {
        Instantiate(_blockTypes.Configs[_hitPoints].HitParticles, transform.position, Quaternion.identity);
    }

}
