using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Spawnable
{
    private bool _isSitting = false;

    private void OnEnable()
    {
        Actions.OnHourChanged += FlyAway;
    }

    private void OnDisable()
    {
        Actions.OnHourChanged -= FlyAway;
    }

    protected override void Move()
    {
        transform.DOMove(_wayPoints[1], 50f / _speed).SetEase(Ease.OutSine).onComplete = (() =>
        {
            _isSitting = true;
            _animator.SetTrigger("Sitting");
        });
    }

    protected override void ScaleForDistanceIllusion()
    {
        return;
    }

    private void FlyAway()
    {
        if (Random.value < 0.3f) return;

        if (_isSitting)
        {
            _isSitting = false;
            _animator.SetTrigger("Flying");
            transform.DOMove(_wayPoints[_wayPoints.Count - 1], 50f / _speed).SetEase(Ease.InSine).onComplete = (() =>
            {
                Destroy(gameObject);
            });
        }
    }
}