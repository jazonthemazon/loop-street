using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineAnimate))]
public class PedestrianBehaviour : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineToFollow;

    [SerializeField] private float _rotationIntensity;

    [SerializeField] private float _size;

    private SplineAnimate _splineAnimate;

    private float _startingY;

    void Awake()
    {
        _splineToFollow = GameObject.Find("Spline 1").GetComponent<SplineContainer>();
        _splineAnimate = GetComponent<SplineAnimate>();
        _splineAnimate.Container = _splineToFollow;
        _splineAnimate.Play();
    }

    private void Start()
    {
        _startingY = transform.position.y;
    }

    private void Update()
    {
        // create depth illusion
        float currentY = transform.position.y;
        transform.localScale = new Vector2(_size + (_startingY - currentY), _size + (_startingY - currentY));

        // rotate player slightly to the left and right of the screen
        float currentX = transform.position.x;
        transform.rotation = Quaternion.Euler(0, 0, -currentX * _rotationIntensity);
    }
}