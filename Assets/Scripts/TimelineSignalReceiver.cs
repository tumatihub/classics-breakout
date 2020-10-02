using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineSignalReceiver : MonoBehaviour
{
    private AudioManager _audioManager;

    void Start()
    {
        _audioManager = AudioManager.Instance;    
    }

    public void StartIntroMusic()
    {
        _audioManager.StartIntroMusic();
    }
}
