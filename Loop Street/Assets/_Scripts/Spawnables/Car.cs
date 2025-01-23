using UnityEngine;

public class Car : Spawnable
{
    protected override void Awake()
    {
        base.Awake();
        _animator.SetInteger("RandomAnimation", Random.Range(0, 6));
    }
}