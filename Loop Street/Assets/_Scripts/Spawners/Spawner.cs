using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawnable")]
    [SerializeField] protected Spawnable _spawnablePrefab;
    [SerializeField][Range(0, 1)] protected float _spawnProbability;

    [Header("Paths")]
    [SerializeField] protected List<Path> _paths;

    [Header("Gizmos")]
    [SerializeField] protected bool _showGizmos;
    [SerializeField] protected Color _gizmoColor;

    protected int _currentCooldown = 0;
    protected List<Vector2> _currentPath;

    protected virtual void OnEnable()
    {
        Actions.OnMinuteChanged += MaybeSpawn;
    }

    protected virtual void OnDisable()
    {
        Actions.OnMinuteChanged -= MaybeSpawn;
    }

    protected virtual void MaybeSpawn()
    {
        if (Random.value > _spawnProbability) return;

        _currentPath = _paths[Random.Range(0, _paths.Count)].Waypoints;
        Spawnable spawnable = Instantiate(_spawnablePrefab, _currentPath[0], Quaternion.identity);
        spawnable.SetWaypoints(_currentPath);
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmos || _paths.Count == 0) return;

        foreach (Path path in _paths)
        {
            if (path.Waypoints.Count == 0) continue;

            Vector2 lastWaypoint = path.Waypoints[0];
            foreach (Vector2 waypoint in path.Waypoints)
            {
                Gizmos.color = _gizmoColor;
                Gizmos.DrawSphere(waypoint, 0.1f);
                Gizmos.DrawLine(lastWaypoint, waypoint);
                lastWaypoint = waypoint;
            }
        }
    }
}