using UnityEngine;

public class Pedestrian : Spawnable
{
    protected override void Awake()
    {
        base.Awake();
        _animator.SetInteger("RandomAnimation", Random.Range(0, 3));
    }
}