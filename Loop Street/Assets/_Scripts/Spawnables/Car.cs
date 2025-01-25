using UnityEngine;

public class Car : Spawnable
{
    protected override void Awake()
    {
        base.Awake();
        _animator.SetInteger("RandomAnimation", Random.Range(0, 5));
    }

    protected override bool RaycastHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _movementDirection, _currentRayLength);

        if (hit)
        {
            if (hit.collider.gameObject.TryGetComponent<CrossingManager>(out CrossingManager crossingManager))
            {
                return crossingManager.PedestriansOnCrossing;
            }
            else
            {
                return true;
            }
        }

        return false;
    }
}