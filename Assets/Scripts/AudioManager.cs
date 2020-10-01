using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private const string MUSIC_VOLUME = "MusicVolume";
    private const string MASTER_VOLUME = "MasterVolume";
    private const string SFX_VOLUME = "SFXVolume";
    private const string MUSIC_PITCH = "MusicPitch";
    private const string SFX_PITCH = "SFXPitch";
    private const float MIN_VOLUME = -80f;
    private const float MAX_VOLUME = 20f;

    private float _musicVolume;
    private float _sFXVolume;
    private float _masterVolume;

    [SerializeField] private BoolVariable _isAudioOn;
    [SerializeField] private BoolVariable _isMusicOn;

    [SerializeField] private AudioClip _gameMusic;
    [SerializeField] private AudioClip _menuMusic;

    [SerializeField] private AudioMixer _audioMixer;

    private static AudioManager _instance;

    public static AudioManager Instance => _instance;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        _audioMixer.GetFloat(MUSIC_VOLUME, out _musicVolume);
        _audioMixer.GetFloat(SFX_VOLUME, out _sFXVolume);
        _audioMixer.GetFloat(MASTER_VOLUME, out _masterVolume);
        _audioSource = GetComponent<AudioSource>();

        if (_isMusicOn.Value) UnmuteMusic();
        else MuteMusic();
        if (_isAudioOn.Value) UnmuteSFX();
        else MuteSFX();
    }

    public void TransitionToMenu()
    {
        TransitionToMusic(_menuMusic, 1f, .5f);
    }

    public void TransitionToGame()
    {
        TransitionToMusic(_gameMusic, 1f, 0.2f);
    }

    public void TransitionToMusic(AudioClip music, float fadeOutTime, float fadeInTime)
    {
        var currentVolume = (_isMusicOn.Value) ? _musicVolume : MIN_VOLUME;
        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.value(gameObject, UpdateVolume, currentVolume, MIN_VOLUME, fadeOutTime).setIgnoreTimeScale(true)
        );
        seq.append( () =>
            {
                _audioSource.clip = music;
                _audioSource.Play();
            }   
        );
        seq.append(
            LeanTween.value(gameObject, UpdateVolume, MIN_VOLUME, currentVolume, fadeInTime).setIgnoreTimeScale(true)
        );
    }

    private void UpdateVolume(float value)
    {
        _audioMixer.SetFloat(MUSIC_VOLUME, value);
    }

    public void PitchDown()
    {
        if (_isAudioOn.Value) _audioMixer.SetFloat(SFX_PITCH, .3f);
        if (_isMusicOn.Value) _audioMixer.SetFloat(MUSIC_PITCH, .9f);
    }

    public void PitchUp()
    {
        if (_isAudioOn.Value) _audioMixer.SetFloat(SFX_PITCH, 1f);
        if (_isMusicOn.Value) _audioMixer.SetFloat(MUSIC_PITCH, 1f);
    }

    public void MuteMusic()
    {
        GroupVolumeDown(MUSIC_VOLUME, _musicVolume, 1);
        _isMusicOn.Value = false;
    }

    public void UnmuteMusic()
    {
        RestoreMusicVolume();
        _isMusicOn.Value = true;
    }

    public void MuteSFX()
    {
        GroupVolumeDown(SFX_VOLUME, _sFXVolume, 1);
        _isAudioOn.Value = false;
    }

    public void UnmuteSFX()
    {
        RestoreGroupVolume(SFX_VOLUME, _sFXVolume);
        _isAudioOn.Value = true;
    }

    public void MusicVolumeDown()
    {
        GroupVolumeDown(MUSIC_VOLUME, _musicVolume, .2f);
    }

    public void RestoreMusicVolume()
    {
        RestoreGroupVolume(MUSIC_VOLUME, _musicVolume);
    }

    public void MasterVolumeDown()
    {
        GroupVolumeDown(MASTER_VOLUME, _masterVolume, .2f);
    }

    public void RestoreMasterVolume()
    {
        RestoreGroupVolume(MASTER_VOLUME, _masterVolume);
    }

    private void GroupVolumeDown(string exposedParameter, float currentVolume, float ammountToReduce)
    {
        _audioMixer.SetFloat(exposedParameter, Mathf.Lerp(currentVolume, MIN_VOLUME, ammountToReduce));
    }

    private void RestoreGroupVolume(string exposedParameter, float previousVolume)
    {
        _audioMixer.SetFloat(exposedParameter, previousVolume);
    }
}
