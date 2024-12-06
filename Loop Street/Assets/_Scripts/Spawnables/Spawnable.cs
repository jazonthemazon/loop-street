using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class Spawnable : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] protected Vector2[] _wayPoints;

    [Header("Size & Scaling")]
    [SerializeField] protected float _size;
    [SerializeField] protected float _scaleFactor;
    [SerializeField] protected float _rotationIntensity;

    protected Animator _animator;
    protected Vector2 _previousPosition;
    protected Vector2 _startPosition;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        transform.localScale *= _size;
        _startPosition = transform.position;
    }

    private void Start()
    {
        _previousPosition = transform.position;

        Sequence sequence = DOTween.Sequence();

        foreach (var wayPoint in _wayPoints)
        {
            sequence.Append(transform.DOMove(wayPoint, 2f).SetEase(Ease.Linear));
        }
    }

    private void Update()
    {
        // Scale spawnable in dependance to y position to simulate depth
        float currentScale = _startPosition.y - transform.position.y + 1;
        transform.localScale = new Vector2 (currentScale, currentScale);

        // rotate spawnable slightly to the left and right of the screen
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
                _animator.SetTrigger("Trigger Side Walk");
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (deltaPosition.x > 0)
            {
                _animator.SetTrigger("Trigger Side Walk");
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            if (deltaPosition.y < 0)
            {
                _animator.SetTrigger("Trigger Front Walk");
            }
            else if (deltaPosition.y > 0)
            {
                _animator.SetTrigger("Trigger Back Walk");
            }
        }

        _previousPosition = currentPosition;
    }

    public void SetWayPoints(Vector2[] waypoints)
    {
        _wayPoints = waypoints;
    }

    public void SetSize(float size)
    {
        transform.localScale = new Vector2(size, size);
    }
}