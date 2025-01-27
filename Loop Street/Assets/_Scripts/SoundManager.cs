using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _slider.onValueChanged.AddListener(delegate { UpdateMusicPitch(); });
        UpdateMusicPitch();
    }

    private void UpdateMusicPitch()
    {
        _audioSource.pitch = _slider.value * 6 - 3;
        Time.timeScale = Mathf.Abs(_audioSource.pitch);
    }
}