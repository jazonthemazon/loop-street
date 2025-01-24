using UnityEngine;

public class Pedestrian : Spawnable
{
    [Header("Weirdness")]
    [SerializeField][Range(0, 1)] private float _chanceForWeirdness;
    [SerializeField] private float _weirdBig;
    [SerializeField] private float _weirdSmall;

    protected override void Awake()
    {
        base.Awake();
        _animator.SetInteger("RandomAnimation", Random.Range(0, 3));

        if (Random.value < _chanceForWeirdness)
        {
            if (Random.Range(0, 2) == 0)
            {
                _size = _weirdBig;
            }
            else
            {
                _size = _weirdSmall;
            }
        }
    }
}