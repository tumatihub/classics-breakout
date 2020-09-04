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
            DissolveToOtherSprite();
            _playerStats.ChargePerHit();
        }
    }

    public void SetHitPoints(int hitPoints)
    {
        _hitPoints = Mathf.Clamp(hitPoints, 1, _blockSprites.Configs.Length);
    }

    private void UpdateSprite()
    {
        if (_hitPoints > _blockSprites.Configs.Length || _hitPoints < 1) return;
        _sprite.sprite = _blockSprites.Configs[_hitPoints - 1].Sprite;
        _sprite.material.SetColor("_Color", _blockSprites.Configs[_hitPoints - 1].DissolveColor);
        _sprite.material.SetFloat("_Offset", UnityEngine.Random.Range(0f, 5f));
    }

    public void RemoveBlock()
    {
        _playerStats.ChargePerRemovedBlock();
        _collider.enabled = false;
        Dissolve();
    }

    private void Dissolve()
    {
        var seq = LeanTween.sequence();
        seq.append(LeanTween.value(gameObject, UpdateDissolveFadeValue, 1f, 0f, .3f));
        seq.append(() => { Destroy(gameObject); });
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
}
