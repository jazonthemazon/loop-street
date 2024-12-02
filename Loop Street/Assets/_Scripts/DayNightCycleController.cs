using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    [SerializeReference] GameObject _skyAtNight;
    [SerializeReference] GameObject _streetAtNight;
    [SerializeReference] GameObject _housesAtNight;
    [SerializeReference] GameObject _houseAtNight;
    
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
            _nightColor.a += 0.01f;
            if (_nightColor.a >= 1) _gettingDark = false;
        }
        else
        {
            _nightColor.a -= 0.01f;
            if (_nightColor.a <= 0) _gettingDark = true;
        }

        UpdateSprites();
    }

    private void UpdateSprites()
    {
        _skyAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _streetAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _housesAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
        _houseAtNight.GetComponent<SpriteRenderer>().color = _nightColor;
    } 
}
