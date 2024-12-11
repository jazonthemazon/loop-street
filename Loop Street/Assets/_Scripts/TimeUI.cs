using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeReference] private TextMeshProUGUI _timeText;

    private void Start()
    {
        UpdateTimeUI();
    }

    private void OnEnable()
    {
        Actions.OnMinuteChanged += UpdateTimeUI;
    }

    private void OnDisable()
    {
        Actions.OnMinuteChanged -= UpdateTimeUI;
    }

    private void UpdateTimeUI()
    {
        _timeText.text = $"Day {TimeManager.Day}  {TimeManager.Hour:00}:{TimeManager.Minute:00}";
    }
}