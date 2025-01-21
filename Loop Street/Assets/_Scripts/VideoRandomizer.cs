using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoRandomizer : MonoBehaviour
{
    [SerializeField] private List<VideoClip> _videoClips;
    [SerializeField] [Range(0, 1)] private float _probabilityPerHour;
    [SerializeField] private float _fadeDuration;

    private SpriteRenderer _spriteRenderer;
    private VideoPlayer _videoPlayer;
    private Color _spriteColorVisible;
    private Color _spriteColorInvisible;
    private bool _visible;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _videoPlayer = GetComponent<VideoPlayer>();
        _visible = false;
        _spriteColorVisible = _spriteRenderer.color;
        _spriteColorInvisible = _spriteRenderer.color;
        _spriteColorVisible.a = 1;
        _spriteColorInvisible.a = 0;
    }

    private void OnEnable()
    {
        Actions.OnHourChanged += ToggleVideo;
    }

    private void OnDisable()
    {
        Actions.OnHourChanged -= ToggleVideo;
    }

    private void ToggleVideo()
    {
        if (Random.value > _probabilityPerHour) return;
        if (!_visible)
        {
            _visible = true;
            _videoPlayer.clip = _videoClips[Random.Range(0, _videoClips.Count)];
            StartCoroutine(Fade(1));
        }
        else
        {
            _visible = false;
            StartCoroutine(Fade(0));
        }
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startValue = _spriteRenderer.color.a;
        float elapsedTime = 0;

        while (elapsedTime < _fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startValue, targetAlpha, elapsedTime / _fadeDuration);
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, newAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}