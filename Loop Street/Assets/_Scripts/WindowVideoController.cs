using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WindowVideoController : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float _probabilityToPlayVideoPerHour;

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
        Actions.OnHourChanged += MaybePlayVideoInOneWindow;
        foreach (var player in videoPlayers)
        {
            player.loopPointReached += DisablePlayer;
        }
    }

    private void OnDisable()
    {
        Actions.OnHourChanged -= MaybePlayVideoInOneWindow;
        foreach (var player in videoPlayers)
        {
            player.loopPointReached -= DisablePlayer;
        }
    }

    private void DisablePlayer(VideoPlayer player)
    {
        player.enabled = false;
    }

    private void MaybePlayVideoInOneWindow()
    {
        if (Random.value < _probabilityToPlayVideoPerHour)
        {
            VideoPlayer videoPlayer = videoPlayers[Random.Range(0, videoPlayers.Count)];
            videoPlayer.enabled = true;
            videoPlayer.Play();
        }
    }
}