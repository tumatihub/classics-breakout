using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Barrier : MonoBehaviour
{
    private const float LINE_MAX_SIZE = 35f;
    private const float BORDER_X_POS = 8.89f;

    [SerializeField] private LineRenderer _leftLine;
    [SerializeField] private LineRenderer _rightLine;
    [SerializeField] private float _lineSpeed;
    [SerializeField] private GameObject _barrierBorderParticles;

    private Vector3 _leftLineStartPos = new Vector3(-.5f, 0f, 0f);
    private Vector3 _rightLineStartPos = new Vector3(.5f, 0f, 0f);

    private Vector3 _leftLinePos;
    private Vector3 _rightLinePos;

    private GameObject _leftBorderParticles;
    private GameObject _rightBorderParticles;

    [SerializeField] ParticleSystem _leftPaddleParticles;
    [SerializeField] ParticleSystem _rightPaddleParticles;
    [SerializeField] Material _lineMaterial;
    [SerializeField] ParticleSystem _lineParticles;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _randomFlickerClip;
    [SerializeField] private AudioClip _flickerTurnoffClip;
    [SerializeField] private AudioClip _laserClip;

    private CinemachineImpulseSource _impulseSource;

    Coroutine _flickerCoroutine;
    private bool _isActive = false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void OnEnable()
    {
        _leftLine.SetPosition(1, _leftLineStartPos);
        _rightLine.SetPosition(1, _rightLineStartPos);

        _leftLine.GetComponent<BoxCollider2D>().enabled = false;
        _rightLine.GetComponent<BoxCollider2D>().enabled = false;

        _lineMaterial.SetInt("_isFading", 0);
        _lineMaterial.SetFloat("_fade", 0);

        _audioSource.clip = _laserClip;
        _audioSource.Play();
        _flickerCoroutine = StartCoroutine(Flicker());
    }

    // Update is called once per frame
    void Update()
    {
        if (_isActive) return;

        _leftLinePos = new Vector3(Mathf.Max(_leftLinePos.x - _lineSpeed * Time.deltaTime,-LINE_MAX_SIZE), 0, 0);
        _rightLinePos = new Vector3(Mathf.Min(_rightLinePos.x + _lineSpeed * Time.deltaTime,LINE_MAX_SIZE), 0, 0);

        _leftLine.SetPosition(1, _leftLinePos);
        if (_leftBorderParticles == null && _leftLinePos.x + transform.position.x <= -BORDER_X_POS)
        {
            _leftBorderParticles = Instantiate(_barrierBorderParticles, new Vector3(-BORDER_X_POS, transform.position.y, transform.position.z), Quaternion.LookRotation(Vector3.right));
            _leftLine.GetComponent<BoxCollider2D>().enabled = true;
            _leftPaddleParticles.Stop();
            _impulseSource.GenerateImpulse(.5f);

        }

        _rightLine.SetPosition(1, _rightLinePos);
        if (_rightBorderParticles == null && _rightLinePos.x + transform.position.x >= BORDER_X_POS)
        {
            _rightBorderParticles = Instantiate(_barrierBorderParticles, new Vector3(BORDER_X_POS, transform.position.y, transform.position.z), Quaternion.LookRotation(-Vector3.right));
            _rightLine.GetComponent<BoxCollider2D>().enabled = true;
            _rightPaddleParticles.Stop();
            _impulseSource.GenerateImpulse(.5f);
        }

        if (_leftLinePos.x + transform.position.x <= -BORDER_X_POS && _rightLinePos.x + transform.position.x >= BORDER_X_POS) _audioSource.Stop();

        if (_leftLinePos.x <= -LINE_MAX_SIZE && _rightLinePos.x >= LINE_MAX_SIZE) _isActive = true;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            TurnOff();
        }    
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (Random.value < .5f)
            {
                _lineMaterial.SetInt("_isFading", 1);
                _audioSource.PlayOneShot(_randomFlickerClip);
                yield return new WaitForSeconds(.5f);
                _lineMaterial.SetInt("_isFading", 0);
            }
        }
    }

    private bool IsCollidingWithBorder(Vector3 pos)
    {
        var collider = Physics2D.OverlapCircle(pos, .5f);
        if (collider != null && collider.CompareTag("Border")) return true;
        return false;
    }

    private void TurnOff()
    {
        var seq = LeanTween.sequence();
        seq.append(()=> 
        {
            var main = _lineParticles.main;
            main.loop = false;
            _lineMaterial.SetInt("_isFading", 1);
            _audioSource.PlayOneShot(_flickerTurnoffClip);
        });
        seq.append(
            LeanTween.value(gameObject, UpdateMaterialFade, 0f, 1f, .5f).setEase(LeanTweenType.easeInCirc)    
        );
        seq.append(()=>
        {
            DeactivateBarrier();
        });

    }

    private void UpdateMaterialFade(float value)
    {
        _lineMaterial.SetFloat("_fade", value);
    }

    private void DeactivateBarrier()
    {
        StopCoroutine(_flickerCoroutine);
        Destroy(_leftBorderParticles.gameObject);
        Destroy(_rightBorderParticles.gameObject);
        _leftLine.SetPosition(1, _leftLineStartPos);
        _leftLinePos = _leftLineStartPos;
        _rightLine.SetPosition(1, _rightLineStartPos);
        _rightLinePos = _rightLineStartPos;
        _isActive = false;
        _lineMaterial.SetInt("_isFading", 0);
        _lineMaterial.SetFloat("_fade", 0);
        var main = _lineParticles.main;
        main.loop = true;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_leftLinePos + gameObject.transform.position, .1f);
        Gizmos.DrawWireSphere(_rightLinePos + gameObject.transform.position, .1f);
    }
}
