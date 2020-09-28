using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    private float _musicVolume;
    private float _sFXVolume;
    [SerializeField] private AudioMixer _audioMixer;

    void Start()
    {
        _audioMixer.GetFloat("MusicVolume", out _musicVolume);
        _audioMixer.GetFloat("SFXVolume", out _sFXVolume);
    }

    public void PitchDown()
    {
        _audioMixer.SetFloat("SFXPitch", .3f);
        _audioMixer.SetFloat("MusicPitch", .9f);
    }

    public void PitchUp()
    {
        _audioMixer.SetFloat("SFXPitch", 1f);
        _audioMixer.SetFloat("MusicPitch", 1f);
    }

    public void MuteMusic()
    {
        _audioMixer.SetFloat("MusicVolume", -80f);
    }

    public void UnmuteMusic()
    {
        RestoreMusicVolume();
    }

    public void MusicVolumeDown()
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Lerp(_musicVolume, -80f, .2f));
    }

    public void RestoreMusicVolume()
    {
        _audioMixer.SetFloat("MusicVolume", _musicVolume);
    }
}
