using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class Spawnable : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] protected List<Vector2> _wayPoints;

    [Header("Size & Scaling")]
    [SerializeField] protected float _size;
    [SerializeField] protected float _scaleOffset;
    [SerializeField] protected float _rotationIntensity;

    protected Animator _animator;
    protected Vector2 _previousPosition;
    protected Tween _currentTween;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _previousPosition = transform.position;

        //Sequence sequence = DOTween.Sequence();
        //
        //foreach (var wayPoint in _wayPoints)
        //{
        //    Tween tween = transform.DOMove(wayPoint, 2f).SetEase(Ease.Linear).SetSpeedBased(true);
        //    sequence.Append(tween);
        //}

        Vector3[] path = new Vector3[_wayPoints.Count];

        int i = 0;
        foreach (var point in _wayPoints)
        {
            path[i] = point;
            i++;
        }

        _currentTween = transform.DOPath(path, 5, PathType.Linear, PathMode.Sidescroller2D, 10, Color.green).SetEase(Ease.Linear);
    }

    private void Update()
    {
        ScaleForDistanceIllusion();

        // rotate spawnable slightly to the left and right of the screen
        transform.rotation = Quaternion.Euler(0, 0, -transform.position.x * _rotationIntensity);

        UpdateAnimation();
    }
    protected virtual void ScaleForDistanceIllusion()
    {
        float currentScale = (_scaleOffset - transform.position.y) * _size * 0.1f;
        currentScale = Mathf.Max(0, currentScale);
        transform.localScale = new Vector2(currentScale, currentScale);
    }

    protected virtual void UpdateAnimation()
    {
        Vector2 currentPosition = transform.position;
        Vector2 deltaPosition = currentPosition - _previousPosition;
        deltaPosition.Normalize();

        if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
        {
            if (deltaPosition.x < 0)
            {
                _animator.SetTrigger("Trigger Side");
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (deltaPosition.x > 0)
            {
                _animator.SetTrigger("Trigger Side");
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            if (deltaPosition.y < 0)
            {
                _animator.SetTrigger("Trigger Front");
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (deltaPosition.y > 0)
            {
                _animator.SetTrigger("Trigger Back");
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        _previousPosition = currentPosition;
    }

    public void SetWaypoints(List<Vector2> waypoints)
    {
        _wayPoints = waypoints;
    }
}