using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] float _speed;

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
        }
    }
}
