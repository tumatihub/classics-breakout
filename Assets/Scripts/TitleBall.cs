using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TitleBall : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] float _speed;
    [SerializeField] private PlayerStats _playerStats;

    public int PiercingCountLeft = 0;
    public bool IsExplosionActivated = false;
    [SerializeField] private List<GameObject> _explosionsPrefabs;
    /*[SerializeField] private UpgradeProgress _explosionProgress;*/

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

    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _ballHits = new List<AudioClip>();

    private CinemachineImpulseSource _impulseSource;

    [SerializeField] private ExplosionSpecial _explosionSpecial;
    [SerializeField] private PiercingSpecial _piercingSpecial;

    [SerializeField] private bool _intro;
    private Vector3 _originalPos;

    public event Action OnDestroy;

    void Start()
    {
        _originalPos = transform.position;
        _ballMaterial = GetComponent<SpriteRenderer>().material;
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.isKinematic = true;
        _audioSource = GetComponent<AudioSource>();
        _aura.Play();
        ChangeTrailToNormal();
        _impulseSource = GetComponent<CinemachineImpulseSource>();

        if (_intro)
        {
            ChangeTrailToSpecial(_piercingSpecial);
            Launch(Vector2.right);
            PiercingCountLeft = 100;
        }
        else
        {
            Launch(new Vector2(1, 1).normalized);
            InvokeRepeating("ChangeSpecial", 10f, 10f);
        }
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
    
    private void ChangeSpecial()
    {
        ChangeTrailToSpecial(_explosionSpecial);
        IsExplosionActivated = true;
    }

    public void Dissolve()
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
        seq.append(() =>
        {
            transform.position = _originalPos;
            _idleTime = 0;
            gameObject.layer = LayerMask.NameToLayer("Default");
            Launch(new Vector2(1, 1).normalized);
        }

        );
        seq.append(
            LeanTween.value(gameObject, UpdateDissolve, 0f, 1f, 1f)
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

        /*if (collision.gameObject.CompareTag("Player"))
        {
            var playerController = collision.gameObject.GetComponent<PlayerController>();
            if (transform.position.y >= playerController.GetComponent<BoxCollider2D>().bounds.center.y)
            {
                _rigidbody.velocity = playerController.ArrowDirection * _speed;
                PlayRandomBallHit();
                _idleTime = 0;
                playerController.ActivateSpecial(this);
            }
        }*/

        /*if (collision.gameObject.CompareTag("Floor"))
        {
            DestroyBall();
        }*/
    }

    void PlayRandomBallHit()
    {
        _audioSource.PlayOneShot(_ballHits[UnityEngine.Random.Range(0, _ballHits.Count)]);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            PlayRandomBallHit();
            _idleTime = 0;
            var block = collision.gameObject.GetComponent<TitleBlock>();
            if (PiercingCountLeft > 0)
            {
                CameraShake(.5f);
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

    private void CameraShake(float force)
    {
        _impulseSource.GenerateImpulse(force);
    }

    private void ChangeTrailToNormal()
    {
        if (_trail != null) Destroy(_trail.gameObject);
        _trail = Instantiate(_normalBallTrail, transform.position, Quaternion.identity, transform);
    }

    public void ChangeTrailToSpecial(Special special)
    {
        if (_trail != null) Destroy(_trail.gameObject);
        _trail = Instantiate(special.BallTrail, transform.position, Quaternion.identity, transform);
    }

    private void DestroyBall()
    {
        OnDestroy?.Invoke();
    }

    public void Explode()
    {
        IsExplosionActivated = false;
        Instantiate(_explosionsPrefabs[2], transform.position, Quaternion.identity);
        Collider2D[] blockList = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y),
            1.5f,
            LayerMask.GetMask("Block")
        );
        foreach (var blockCollider in blockList)
        {
            TitleBlock block = blockCollider.GetComponent<TitleBlock>();
            block.RemoveBlock();
        }
    }

    public void Launch(Vector2 dir)
    {
        _aura.Stop();
        _rigidbody.isKinematic = false;
        _rigidbody.velocity = dir * _speed;
    }
}
