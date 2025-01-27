using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private int _startHour;
    [SerializeField] private float _minutesPerSecond;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; }

    private float _timer;

    void Start()
    {
        Minute = 0;
        Hour = _startHour;
        Day = 1;
        _timer = 1 / _minutesPerSecond;
    }
    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            Minute++;
            Actions.OnMinuteChanged?.Invoke();

            if (Minute >= 60)
            {
                Hour++;
                Actions.OnHourChanged?.Invoke();

                if (Hour >= 24)
                {
                    Day++;
                    Actions.OnDayChanged?.Invoke();
                    Hour = 0;
                }

                Minute = 0;
            }

            _timer = 1 / _minutesPerSecond;
        }
    }
}