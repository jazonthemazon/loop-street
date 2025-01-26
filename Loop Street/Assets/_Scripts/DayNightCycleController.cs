using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    [Header("References")]
    [SerializeReference] private GameObject _skyAtNight;
    [SerializeReference] private GameObject _streetAtNight;
    [SerializeReference] private GameObject _housesAtNight;
    [SerializeReference] private GameObject _houseAtNight;
    [SerializeReference] private GameObject _streetLights;

    [Header("Street Lights")]
    [SerializeField] float _streetLightsSwitchDarkness;

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
        Actions.OnHourChanged += HourChange;
        Actions.OnMinuteChanged += UpdateAlpha;
    }

    private void OnDisable()
    {
        Actions.OnHourChanged -= HourChange;
        Actions.OnMinuteChanged -= UpdateAlpha;
    }

    void Start()
    {
        _nightColor = _streetAtNight.GetComponent<SpriteRenderer>().color;
        _nightColor.a = 1;

        UpdateSpritesColor();
    }

    private void HourChange()
    {
        if (TimeManager.Hour == _sunriseBeginning)
        {
            StartSunrise();
        }
        else if (TimeManager.Hour == _sunsetBeginning)
        {
            StartSunset();
        }
        else if (TimeManager.Hour == _sunriseEnd)
        {
            EndSunrise();
        }
        else if (TimeManager.Hour == _sunsetEnd)
        {
            EndSunset();
        }
    }

    private void SwitchStreetLights(bool state)
    {
        _streetLights.SetActive(state);
    }

    private void StartSunrise()
    {
        _alphaChangePerMinute = -1f / ((_sunriseEnd - _sunriseBeginning) * 60f);
    }

    private void EndSunrise()
    {
        _nightColor.a = 0f;
        _alphaChangePerMinute = 0f;
    }

    private void StartSunset()
    {
        _alphaChangePerMinute = 1f / ((_sunsetEnd - _sunsetBeginning) * 60f);
    }

    private void EndSunset()
    {
        _nightColor.a = 1f;
        _alphaChangePerMinute = 0f;
    }

    private void UpdateAlpha()
    {
        if (_alphaChangePerMinute == 0f) return;

        _nightColor.a += _alphaChangePerMinute;
        UpdateSpritesColor();

        if (_nightColor.a < _streetLightsSwitchDarkness)
        {
            SwitchStreetLights(false);
        }
        else
        {
            SwitchStreetLights(true);
        }
    }

    private void UpdateSpritesColor()
    {
        _skyAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _streetAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _housesAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _houseAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
    } 
}