using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected Spawnable _spawnablePrefab;
    [SerializeField] protected List<Vector2> _waypoints;
    [SerializeField] protected bool _showGizmos;

    protected int _currentCooldown = 0;

    private void OnEnable()
    {
        Actions.OnHourChanged += Spawn;
    }

    private void OnDisable()
    {
        Actions.OnHourChanged -= Spawn;
    }

    protected virtual void Spawn()
    {
        Spawnable spawnable = Instantiate(_spawnablePrefab, _waypoints[0], Quaternion.identity);
        spawnable.SetWaypoints(_waypoints);
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmos || _waypoints.Count == 0) return;

        Vector2 lastPosition = _waypoints[0];
        foreach (Vector2 position in _waypoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(position, 0.1f);
            Gizmos.DrawLine(lastPosition, position);
            lastPosition = position;
        }
    }
}