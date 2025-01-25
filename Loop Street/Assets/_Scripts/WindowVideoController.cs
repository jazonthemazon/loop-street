using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WindowVideoController : MonoBehaviour
{
    [SerializeField][Range(0, 1)] private float _probabilityToPlayVideoPerMinute;
    [SerializeField][Range(0, 24)] private int _startTime;
    [SerializeField][Range(0, 24)] private int _endTime;

    private List<VideoPlayer> videoPlayers;

    private void Awake()
    {
        videoPlayers = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            VideoPlayer videoPlayer = transform.GetChild(i).GetComponent<VideoPlayer>();
            videoPlayer.enabled = false;
            videoPlayers.Add(videoPlayer);
        }
    }

    private void OnEnable()
    {
        Actions.OnMinuteChanged += MaybePlayVideoInOneWindow;
        foreach (var player in videoPlayers)
        {
            player.loopPointReached += DisablePlayer;
        }
    }

    private void OnDisable()
    {
        Actions.OnMinuteChanged -= MaybePlayVideoInOneWindow;
        foreach (var player in videoPlayers)
        {
            player.loopPointReached -= DisablePlayer;
        }
    }

    private void DisablePlayer(VideoPlayer player)
    {
        player.enabled = false;
        SpriteRenderer renderer = player.gameObject.GetComponent<SpriteRenderer>();
        Sprite tempSprite = renderer.sprite;
        renderer.sprite = null;
        renderer.sprite = tempSprite;
    }

    private void MaybePlayVideoInOneWindow()
    {
        if (TimeManager.Hour >= _endTime && TimeManager.Hour < _startTime) return;
        if (Random.value < _probabilityToPlayVideoPerMinute)
        {
            VideoPlayer videoPlayer = videoPlayers[Random.Range(0, videoPlayers.Count)];
            videoPlayer.enabled = true;
            videoPlayer.Play();
        }
    }
}