using DG.Tweening;
using UnityEngine;

public class Bird : Spawnable
{
    [Header("Bird Variables")]
    [SerializeField][Range(0, 1)] private float _flyAwayProbabilityPerMinute;
    [SerializeField] private Ease _landEase;
    [SerializeField] private Ease _flyAwayEase;

    private bool _isSitting = false;

    protected override void Awake()
    {
        base.Awake();
        _animator.SetInteger("RandomAnimation", Random.Range(0, 7));
    }

    protected override void Start()
    {
        base.Start();
        StartMoving();
    }

    protected override bool RaycastHit()
    {
        return false;
    }

    private void OnEnable()
    {
        Actions.OnMinuteChanged += MaybeFlyAway;
    }

    private void OnDisable()
    {
        Actions.OnMinuteChanged -= MaybeFlyAway;
    }

    protected override void Move()
    {

    }

    private void StartMoving()
    {
        transform.DOMove(_wayPoints[1], 50f / _speed).SetEase(_landEase).OnComplete(() =>
        {
            _isSitting = true;
            _animator.SetTrigger("Sitting");
        });
    }

    private void MaybeFlyAway()
    {
        if (Random.value > _flyAwayProbabilityPerMinute) return;
        if (_isSitting)
        {
            if (Random.value < 0.3f) return;

            _isSitting = false;
            _animator.SetTrigger("Flying");
            transform.DOMove(_wayPoints[_wayPoints.Count - 1], 50f / _speed).SetEase(_landEase).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }

    protected override void UpdateAnimation()
    {
        if (_isSitting) return;

        Vector2 currentPosition = transform.position;
        _movementDirection = currentPosition - _previousPosition;
        _movementDirection.Normalize();

        if (_movementDirection.x < 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_movementDirection.x > 0)
        {
            _spriteRenderer.flipX = true;
        }

        _previousPosition = currentPosition;
    }
}