using UnityEngine;

public class CrossingManager : MonoBehaviour
{
    [SerializeField] private float _cooldown;

    private float _currentCooldown;
    private bool _pedestriansOnCrossing;

    public bool PedestriansOnCrossing => _pedestriansOnCrossing;

    public void SetPedestriansOnCrossing()
    {
        _currentCooldown = _cooldown;
    }

    private void Update()
    {
        if (_currentCooldown <= 0)
        {
            _pedestriansOnCrossing = false;
        }
        else
        {
            _pedestriansOnCrossing = true;
            _currentCooldown = Mathf.Max(_currentCooldown - Time.deltaTime, 0);
        }
    }
}