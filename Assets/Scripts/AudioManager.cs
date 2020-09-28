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

    [SerializeField] private AudioMixer _audioMixer;

    void Start()
    {
        _audioMixer.GetFloat(MUSIC_VOLUME, out _musicVolume);
        _audioMixer.GetFloat(SFX_VOLUME, out _sFXVolume);
        _audioMixer.GetFloat(MASTER_VOLUME, out _masterVolume);
    }

    public void PitchDown()
    {
        _audioMixer.SetFloat(SFX_PITCH, .3f);
        _audioMixer.SetFloat(MUSIC_PITCH, .9f);
    }

    public void PitchUp()
    {
        _audioMixer.SetFloat(SFX_PITCH, 1f);
        _audioMixer.SetFloat(MUSIC_PITCH, 1f);
    }

    public void MuteMusic()
    {
        GroupVolumeDown(MUSIC_VOLUME, _musicVolume, 1);
    }

    public void UnmuteMusic()
    {
        RestoreMusicVolume();
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
