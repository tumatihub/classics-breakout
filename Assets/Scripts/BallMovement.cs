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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = new Vector2(1f, 1f).normalized * _speed;
    }

    public void BounceBall(Vector2 inNormal)
    {
        Debug.DrawLine(transform.position, transform.position + new Vector3(inNormal.x, inNormal.y, transform.position.z) * 5f, Color.red, 2f);
        _rigidbody.velocity = Vector2.Reflect(_rigidbody.velocity, inNormal);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var dir = (transform.position - collision.transform.position).normalized;
            _rigidbody.velocity = dir * _speed;
            collision.gameObject.GetComponent<PlayerController>().ActivateSpecial(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            var block = collision.gameObject.GetComponent<Block>();
            if (PiercingCountLeft > 0)
            {
                PiercingCountLeft--;
                block.RemoveBlock();
                return;
            }

            var normal = (new Vector2(transform.position.x, transform.position.y) - collision.ClosestPoint(transform.position)).normalized;
            BounceBall(normal);
            if (IsExplosionActivated)
            {
                Explode();
                return;
            }
            block.Hit();
        }
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
            block.Hit();
        }
    }
}
