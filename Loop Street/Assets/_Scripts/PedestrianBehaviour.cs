using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

public class PedestrianBehaviour : MonoBehaviour
{
    [SerializeField] private float _rotationIntensity;

    private SplineAnimate _splineAnimate;
    private Animator _walkAnimator;
    private Vector2 _previousPosition;

    void Awake()
    {
        _walkAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _previousPosition = transform.position;
    }

    private void Update()
    {
        // TODO: Scale pedestrians in dependance to y position to simulate depth

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

    public void SetSize(float size)
    {
        transform.localScale = new Vector3(size, size, size);
    }
}