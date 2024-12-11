using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    [Header("References")]
    [SerializeReference] private GameObject _skyAtNight;
    [SerializeReference] private GameObject _streetAtNight;
    [SerializeReference] private GameObject _housesAtNight;
    [SerializeReference] private GameObject _houseAtNight;

    [Header("Sunrise and Sunset")]
    [SerializeField] private int _sunriseBeginning;
    [SerializeField] private int _sunriseEnd;
    [SerializeField] private int _sunsetBeginning;
    [SerializeField] private int _sunsetEnd;

    [Header("Current Alpha")]
    [SerializeField] private Color _nightColor;

    private float _alphaChangePerMinute = 0f;

    private void OnEnable()
    {
        Actions.OnHourChanged += StartSunriseOrSunset;
        Actions.OnMinuteChanged += UpdateAlpha;
    }

    private void OnDisable()
    {
        Actions.OnHourChanged -= StartSunriseOrSunset;
        Actions.OnMinuteChanged -= UpdateAlpha;
    }

    void Start()
    {
        _nightColor = _streetAtNight.GetComponent<SpriteRenderer>().color;
        _nightColor.a = 1;

        UpdateSpritesColor();
    }

    private void StartSunriseOrSunset()
    {
        if (TimeManager.Hour == _sunriseBeginning)
        {
            _alphaChangePerMinute = -1f / ((_sunriseEnd - _sunriseBeginning) * 60f);
        }
        else if (TimeManager.Hour == _sunsetBeginning)
        {
            _alphaChangePerMinute = 1f / ((_sunsetEnd - _sunsetBeginning) * 60f);
        }
        else if (TimeManager.Hour == _sunriseEnd)
        {
            _nightColor.a = 0f;
            _alphaChangePerMinute = 0f;
        }
        else if (TimeManager.Hour == _sunsetEnd)
        {
            _nightColor.a = 1f;
            _alphaChangePerMinute = 0f;
        }
    }

    private void UpdateAlpha()
    {
        _nightColor.a += _alphaChangePerMinute;
        UpdateSpritesColor();
    }

    private void UpdateSpritesColor()
    {


        _skyAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _streetAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _housesAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _houseAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
    } 
}