using System.Collections.Generic;
using UnityEngine;

public abstract class Spawnable : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] protected float _speed;

    [Header("Size & Scaling")]
    [SerializeField] protected float _size;
    [SerializeField] protected bool _scaleBasedOnYCoordinate;
    [SerializeField] protected float _scaleOffset;
    [SerializeField] protected float _rotationIntensity;

    [Header("Raycast Variables")]
    [SerializeField] protected float _rayLength;
    [SerializeField] protected Color _rayColor;

    protected float _currentRayLength;
    protected GameObject _spriteGameObject;
    protected SpriteRenderer _spriteRenderer;
    protected Animator _animator;
    protected Vector2 _previousPosition;
    protected Vector2 _movementDirection;
    protected List<Vector2> _wayPoints;
    protected Vector2 _nextWayPoint;
    protected int _nextWayPointIndex;

    protected virtual void Awake()
    {
        _spriteGameObject = transform.GetChild(0).gameObject;
        _spriteRenderer = _spriteGameObject.GetComponent<SpriteRenderer>();
        _animator = _spriteGameObject.GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        float startingScale = _spriteGameObject.transform.localScale.x * _size;
        _spriteGameObject.transform.localScale = new(startingScale, startingScale, startingScale);
        ScaleForDistanceIllusion();
        RotateSpawnable();

        _previousPosition = transform.position;
        _nextWayPointIndex = 1;
        _nextWayPoint = _wayPoints[_nextWayPointIndex];
    }

    private void Update()
    {


        if (RaycastHit())
        {
            return;
        }
        else
        {
            Move();
            ScaleForDistanceIllusion();
            RotateSpawnable();
            UpdateAnimation();
        }

        _currentRayLength = _rayLength * Mathf.Abs(1 - Mathf.Abs(_movementDirection.y));
    }

    protected abstract bool RaycastHit();

    protected virtual void Move()
    {
        if (Vector2.Distance(transform.position, _nextWayPoint) < 0.1f)
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

        float currentScale = (_scaleOffset - _spriteGameObject.transform.position.y) * _size * 0.1f;
        currentScale = Mathf.Max(0, currentScale);
        _spriteGameObject.transform.localScale = new(currentScale, currentScale);
    }

    private void RotateSpawnable()
    {
        _spriteGameObject.transform.rotation = Quaternion.Euler(0, 0, -_spriteGameObject.transform.position.x * _rotationIntensity);
    }

    protected virtual void UpdateAnimation()
    {
        Vector2 currentPosition = transform.position;
        _movementDirection = currentPosition - _previousPosition;
        _movementDirection.Normalize();

        if (Mathf.Abs(_movementDirection.x) > 0.9f)
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
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)_movementDirection * _currentRayLength);
    }
}