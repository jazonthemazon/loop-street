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
        int randomNumber;

        if (DayNightCycleController.CatastropheIsHappening)
        {
            randomNumber = Random.Range(16, 17);
        }
        else
        {
            randomNumber = Random.Range(0, 17);
        }

        _animator.SetInteger("RandomAnimation", randomNumber);

        if (randomNumber >= 12) _rotationIntensity = 0f;

        if (Random.value < _chanceForWeirdness)
        {
            if (Random.value < 0.5f)
            {
                _size = _weirdBig;
            }
            else
            {
                _size = _weirdSmall;
            }
        }
    }

    protected override bool RaycastHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _movementDirection, _currentRayLength);

        if (hit)
        {
            if (hit.collider.gameObject.TryGetComponent<CrossingManager>(out CrossingManager crossingManager))
            {
                crossingManager.SetPedestriansOnCrossing();
                return false;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    void LateUpdate()
    {
        if (transform.position.y > -2.5f)
        {
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        else
        {
            _spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }
    }
}