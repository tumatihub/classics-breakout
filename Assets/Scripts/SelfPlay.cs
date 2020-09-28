using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SelfPlay : MonoBehaviour
{
    [SerializeField] private float _delay;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }
    void Start()
    {
        Invoke("Play", _delay);
    }

    void Play()
    {
        _audioSource.Play();
    }
}
