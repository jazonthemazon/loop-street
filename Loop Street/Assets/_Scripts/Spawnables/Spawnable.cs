using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] protected float _speed;

    [Header("Path")]
    [SerializeField] protected List<Vector2> _wayPoints;

    [Header("Size & Scaling")]
    [SerializeField] protected float _size;
    [SerializeField] protected bool _scaleBasedOnYCoordinate;
    [SerializeField] protected float _scaleOffset;
    [SerializeField] protected float _rotationIntensity;

    [Header("Raycast Variables")]
    [SerializeField] private float _rayLength;
    [SerializeField] private Color _rayColor;
    [SerializeField] private float _waitDuration;

    protected GameObject _spriteGameObject;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    protected Vector2 _previousPosition;
    protected Vector2 _movementDirection;
    protected Vector2 _nextWayPoint;
    protected int _nextWayPointIndex;
    protected float _cooldownCounter;

    void Awake()
    {
        _spriteGameObject = transform.GetChild(0).gameObject;
        _spriteRenderer = _spriteGameObject.GetComponent<SpriteRenderer>();
        _animator = _spriteGameObject.GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        float startingScale = transform.localScale.x * _size;
        transform.localScale = new(startingScale, startingScale, startingScale);
        ScaleForDistanceIllusion();
        RotateSpawnable();

        _previousPosition = transform.position;
        _nextWayPointIndex = 1;
        _nextWayPoint = _wayPoints[_nextWayPointIndex];
        _cooldownCounter = 0f;
    }

    private void Update()
    {
        RaycastHit2D hit = (Physics2D.Raycast(transform.position, _movementDirection, _rayLength * transform.localScale.x));

        if (hit)
        {
            _cooldownCounter = _waitDuration;
            return;
        }
        else if (_cooldownCounter == 0f)
        {
            Move();
            ScaleForDistanceIllusion();
            RotateSpawnable();
            UpdateAnimation();
        }
        else
        {
            _cooldownCounter = Mathf.Max(_cooldownCounter - Time.deltaTime, 0);
        }
    }

    protected virtual void Move()
    {
        if (Vector2.Distance(transform.position, _nextWayPoint) < 0.2f)
        {
            if (_nextWayPointIndex == _wayPoints.Count - 1)
            {
                Destroy(gameObject);
                return;
            }

            _nextWayPointIndex++;
            _nextWayPoint = _wayPoints[_nextWayPointIndex];
        }

        if (_scaleBasedOnYCoordinate)
        {
            transform.position += (_scaleOffset - transform.position.y) * _speed * 0.2f * Time.deltaTime * ((Vector3)_nextWayPoint - transform.position).normalized;
        }
        else
        {
            transform.position += _speed * 0.2f * Time.deltaTime * ((Vector3)_nextWayPoint - transform.position).normalized;
        }
    }

    protected virtual void ScaleForDistanceIllusion()
    {
        if (!_scaleBasedOnYCoordinate) return;

        float currentScale = (_scaleOffset - transform.position.y) * _size * 0.1f;
        currentScale = Mathf.Max(0, currentScale);
        transform.localScale = new(currentScale, currentScale);
    }

    private void RotateSpawnable()
    {
        _spriteGameObject.transform.rotation = Quaternion.Euler(0, 0, -transform.position.x * _rotationIntensity);
    }

    protected virtual void UpdateAnimation()
    {
        Vector2 currentPosition = transform.position;
        _movementDirection = currentPosition - _previousPosition;
        _movementDirection.Normalize();

        if (Mathf.Abs(_movementDirection.x) > Mathf.Abs(_movementDirection.y))
        {
            if (_movementDirection.x < 0)
            {
                _animator.SetTrigger("Trigger Side");
                _spriteRenderer.flipX = false;
            }
            else if (_movementDirection.x > 0)
            {
                _animator.SetTrigger("Trigger Side");
                _spriteRenderer.flipX = true;
            }
        }
        else
        {
            if (_movementDirection.y < 0)
            {
                _animator.SetTrigger("Trigger Front");
                _spriteRenderer.flipX = false;
            }
            else if (_movementDirection.y > 0)
            {
                _animator.SetTrigger("Trigger Back");
                _spriteRenderer.flipX = false;
            }
        }

        _previousPosition = currentPosition;
    }

    public void SetWaypoints(List<Vector2> waypoints)
    {
        _wayPoints = waypoints;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_movementDirection * _rayLength * transform.localScale.x);
    }
}