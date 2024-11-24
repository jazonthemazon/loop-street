using JetBrains.Annotations;
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
    private Animator _walkAnimator;
    private Vector2 _previousPosition;

    private float _startingY;

    void Awake()
    {
        _splineToFollow = GameObject.Find("Spline 1").GetComponent<SplineContainer>();
        _splineAnimate = GetComponent<SplineAnimate>();
        _splineAnimate.Container = _splineToFollow;
        _splineAnimate.Play();
        _walkAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _previousPosition = transform.position;
    }

    private void Update()
    {
        // create depth illusion
        float currentY = transform.position.y;
        transform.localScale = new Vector2(_size * (- currentY), _size * (- currentY));

        // rotate player slightly to the left and right of the screen
        float currentX = transform.position.x;
        transform.rotation = Quaternion.Euler(0, 0, -currentX * _rotationIntensity);

        // change animation depending on movement direction
        Vector2 currentPosition = transform.position;
        Vector2 deltaPosition = currentPosition - _previousPosition;
        deltaPosition.Normalize();

        if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
        {
            if (deltaPosition.x < 0)
            {
                _walkAnimator.SetTrigger("Trigger Side Walk");
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (deltaPosition.x > 0)
            {
                _walkAnimator.SetTrigger("Trigger Side Walk");
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            if (deltaPosition.y < 0)
            {
                _walkAnimator.SetTrigger("Trigger Front Walk");
            }
            else if (deltaPosition.y > 0)
            {
                _walkAnimator.SetTrigger("Trigger Back Walk");
            }
        }

        _previousPosition = currentPosition;
    }
}