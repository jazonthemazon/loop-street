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
    [SerializeField] protected int _cooldown;
    [SerializeField] protected List<Vector2> _waypoints;

    protected int _currentCooldown = 0;

    void Update()
    {
        if (_currentCooldown == 0)
        {
            int randomNumber = Random.Range(1, 101);
            if (randomNumber <= 1)
            {
                Spawn();
                _currentCooldown = _cooldown;
            }
        }
        _currentCooldown = Mathf.Max(_currentCooldown - 1, 0);
    }

    protected virtual void Spawn()
    {
        Spawnable spawnable = Instantiate(_spawnablePrefab, _waypoints[0], Quaternion.identity);
        spawnable.SetWaypoints(_waypoints);
    }

    private void OnDrawGizmos()
    {
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