using DG.Tweening;
using UnityEngine;

public class Bird : Spawnable
{
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
        Actions.OnMinuteChanged += FlyAway;
    }

    private void OnDisable()
    {
        Actions.OnMinuteChanged -= FlyAway;
    }

    protected override void Move()
    {
        
    }

    private void StartMoving()
    {
        transform.DOMove(_wayPoints[1], 50f / _speed).SetEase(Ease.OutSine).onComplete = (() =>
        {
            _isSitting = true;
            _animator.SetTrigger("Sitting");
        });
    }

    private void FlyAway()
    {
        if (_isSitting)
        {
            if (Random.value < 0.3f) return;

            _isSitting = false;
            _animator.SetTrigger("Flying");
            transform.DOMove(_wayPoints[_wayPoints.Count - 1], 50f / _speed).SetEase(Ease.InSine).onComplete = (() =>
            {
                Destroy(gameObject);
            });
        }
    }
}