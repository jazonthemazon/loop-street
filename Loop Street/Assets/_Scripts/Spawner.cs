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
    [SerializeField] Spawnable _spawnablePrefab;
    [SerializeField] int _cooldown;
    [SerializeField] Vector2[] _wayPoints;

    private int currentCooldown = 0;

    void Update()
    {
        if (currentCooldown == 0)
        {
            int randomNumber = Random.Range(1, 101);
            if (randomNumber <= 1)
            {
                Spawnable spawnable = Instantiate(_spawnablePrefab, _wayPoints[0], Quaternion.identity);
                spawnable.SetWayPoints(_wayPoints);

                currentCooldown = _cooldown;
            }
        }
        currentCooldown = Mathf.Max(currentCooldown - 1, 0);
    }

    private void OnDrawGizmos()
    {
        Vector2 lastPosition = _wayPoints[0];
        foreach (Vector2 position in _wayPoints)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(position, 0.1f);
            Gizmos.DrawLine(lastPosition, position);
            lastPosition = position;
        }
    }
}