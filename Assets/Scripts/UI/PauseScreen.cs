using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private int _secondsToUnpause = 3;
    [SerializeField] private Canvas _pauseCanvas;
    [SerializeField] private TMP_Text _counter;
    [SerializeField] private Image _pauseIcon;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private AudioClip _countdownClip;
    private AudioSource _audioSource;

    private void Awake()
    {
        _pauseCanvas.gameObject.SetActive(false);
        _audioSource = GetComponent<AudioSource>();
    }

    public void Pause()
    {
        _pauseCanvas.gameObject.SetActive(true);
        _counter.enabled = false;
        _pauseIcon.enabled = true;
    }

    public void Unpause()
    {
        _pauseIcon.enabled = false;
        StartCoroutine(StartCounter());
    }

    IEnumerator StartCounter()
    {
        _counter.enabled = true;
        int seconds = _secondsToUnpause;
        while (seconds >= 0)
        {
            _audioSource.PlayOneShot(_countdownClip);
            _counter.text = seconds.ToString();
            yield return new WaitForSecondsRealtime(1);
            seconds--;
        }
        _pauseCanvas.gameObject.SetActive(false);
        _playerController.ExitPauseMode();
    }
}
