using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawnable : MonoBehaviour
{
    public Vector2[] _wayPoints { get; set; }

    private void Start()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var wayPoint in _wayPoints)
        {
            sequence.Append(transform.DOMove(wayPoint, 2f).SetEase(Ease.Linear));
        }
    }
}