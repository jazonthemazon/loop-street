using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeField] GameObject _streetAtNight;
    [SerializeField] GameObject _streetLayerAtNight;
    [SerializeField] Color _nightColor;

    bool _gettingDark;
    

    void Start()
    {
        _gettingDark = true;
        _nightColor = _streetAtNight.GetComponent<SpriteRenderer>().color;
        _nightColor.a = 0;

        UpdateSprites();
    }

    void Update()
    {
        if (_gettingDark)
        {
            _nightColor.a += 0.0001f;
            if (_nightColor.a >= 1) _gettingDark = false;
        }
        else
        {
            _nightColor.a -= 0.0001f;
            if (_nightColor.a <= 0) _gettingDark = true;
        }

        UpdateSprites();
    }

    private void UpdateSprites()
    {
        _streetAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _streetLayerAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
    } 
}
