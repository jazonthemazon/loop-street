using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class Spawner : MonoBehaviour
{
    [SerializeReference] protected Spawnable _spawnablePrefab;
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
        if (!_showGizmos) return;

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