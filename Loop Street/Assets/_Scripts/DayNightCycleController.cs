using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    [Header("References")]
    [SerializeReference] private GameObject _skyAtNight;
    [SerializeReference] private GameObject _streetAtNight;
    [SerializeReference] private GameObject _housesAtNight;
    [SerializeReference] private GameObject _houseAtNight;
    [SerializeReference] private GameObject _streetLights;
    [SerializeReference] private GameObject _windows;

    [Header("Street Lights")]
    [SerializeField] float _streetLightsSwitchDarkness;

    [Header("Sunrise and Sunset")]
    [SerializeField] private int _sunriseBeginning;
    [SerializeField] private int _sunriseEnd;
    [SerializeField] private int _sunsetBeginning;
    [SerializeField] private int _sunsetEnd;

    [Header("Current Alpha")]
    [SerializeField] private Color _nightColor;

    [Header("Random Events")]
    [Header("Catastrophe")]
    [SerializeField][Range(0, 1)] private float _catastropheChancePerHour;
    [SerializeField][Range(0, 1)] private float _catastropheSpawnChance;
    public static bool CatastropheIsHappening = false;
    [SerializeField] private AlterationSpawner _pedestrianSpawner;

    [Header("ManyPeople")]
    [SerializeField][Range(0, 1)] private float _manyPeopleChancePerHour;
    [SerializeField][Range(0, 1)] private float _manyPeopleSpawnChance;
    public static bool ManyPeopleIsHappening = false;

    [Header("ManyCars")]
    [SerializeField][Range(0, 1)] private float _manyCarsChancePerHour;
    [SerializeField][Range(0, 1)] private float _manyCarsSpawnChance;
    public static bool ManyCarsIsHappening = false;
    [SerializeField] private AlterationSpawner _carSpawner;

    private float _initialPedestrianSpawnChance;
    private float _initialCarSpawnChance;

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

    private void Start()
    {
        _nightColor = _streetAtNight.GetComponent<SpriteRenderer>().color;
        _nightColor.a = 1;

        UpdateSpritesColor();
        _initialPedestrianSpawnChance = _pedestrianSpawner.SpawnProbability;
        _initialCarSpawnChance = _carSpawner.SpawnProbability;
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

        if (CatastropheIsHappening)
        {
            CatastropheIsHappening = false;
            _pedestrianSpawner.SpawnProbability = _initialPedestrianSpawnChance;
        }
        if (Random.value < _catastropheChancePerHour)
        {
            CatastropheIsHappening = true;
            _pedestrianSpawner.SpawnProbability = _catastropheSpawnChance;
        }

        if (ManyPeopleIsHappening)
        {
            ManyPeopleIsHappening = false;
            _pedestrianSpawner.SpawnProbability = _initialPedestrianSpawnChance;
        }
        if (Random.value < _manyPeopleChancePerHour)
        {
            ManyPeopleIsHappening = true;
            _pedestrianSpawner.SpawnProbability = _manyPeopleSpawnChance;
        }

        if (ManyCarsIsHappening)
        {
            ManyCarsIsHappening = false;
            _carSpawner.SpawnProbability = _initialCarSpawnChance;
        }
        if (Random.value < _manyCarsChancePerHour)
        {
            ManyCarsIsHappening = true;
            _carSpawner.SpawnProbability = _manyCarsSpawnChance;
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

        for (int x = 0; x < _windows.transform.childCount; x++)
        {
            _windows.transform.GetChild(x).GetComponent<SpriteRenderer>().color = _nightColor;
        }
    }
}