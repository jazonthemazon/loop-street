using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : Spawner
{
    [SerializeField][Range(0, 1)] private float _spawnProbability;
    [SerializeField] private List<Vector2> _possibleRestSpots = new();

    protected override void Spawn()
    {
        if (Random.value > _spawnProbability) return;

        _currentPath = new();

        _currentPath.Add(PointOnLeftOrRightOfScreen());

        int randomNumber = Random.Range(0, _possibleRestSpots.Count);
        _currentPath.Add(_possibleRestSpots[randomNumber]);

        _currentPath.Add(PointOnLeftOrRightOfScreen());

        Spawnable spawnable = Instantiate(_spawnablePrefab, _currentPath[0], Quaternion.identity);
        spawnable.SetWaypoints(_currentPath);
    }

    private Vector2 PointOnLeftOrRightOfScreen()
    {
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * (16f / 9f) + 1f;
        bool onTheRight = Random.value < 0.5f;
        
        if (!onTheRight)
        {
            return new(Random.Range(-screenWidth, -screenWidth * 0.5f), Random.Range(0f, screenHeight * 0.5f));
        }
        else
        {
            return new(Random.Range(screenWidth, screenWidth * 0.5f), Random.Range(0f, screenHeight * 0.5f));
        }
    }

    private void OnDrawGizmos()
    {
        if (!_showGizmos) return;

        Gizmos.color = _gizmoColor;
        foreach (Vector2 point in _possibleRestSpots)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}