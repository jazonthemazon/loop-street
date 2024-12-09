using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeReference] private TextMeshProUGUI _timeText;
    private void OnEnable()
    {
        TimeManager.OnMinuteChanged += UpdateTimeUI;
        TimeManager.OnHourChanged += UpdateTimeUI;
    }

    private void OnDisable()
    {
        TimeManager.OnMinuteChanged -= UpdateTimeUI;
        TimeManager.OnHourChanged -= UpdateTimeUI;
    }

    private void UpdateTimeUI()
    {
        _timeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}";
    }
}