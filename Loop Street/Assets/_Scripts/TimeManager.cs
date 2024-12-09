using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float _minutesPerSecond = 1f;

    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    private float _timer;

    void Start()
    {
        Minute = 0;
        Hour = 0;
        _timer = 1 / _minutesPerSecond;
    }
    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            Minute++;
            OnMinuteChanged?.Invoke();

            if (Minute >= 60)
            {
                Hour++;
                Minute = 0;
                OnHourChanged?.Invoke();
            }

            _timer = 1 / _minutesPerSecond;
        }
    }
}