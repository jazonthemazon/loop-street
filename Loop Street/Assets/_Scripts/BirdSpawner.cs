using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : Spawner
{
    [SerializeField] private List<Vector2> _possibleRestSpots = new();

    protected override void Spawn()
    {
        _waypoints = new();

        _waypoints.Add(PointOnLeftOrRightOfScreen());

        int randomNumber = Random.Range(0, _possibleRestSpots.Count);
        _waypoints.Add(_possibleRestSpots[randomNumber]);

        _waypoints.Add(PointOnLeftOrRightOfScreen());

        Spawnable spawnable = Instantiate(_spawnablePrefab, _waypoints[0], Quaternion.identity);
        spawnable.SetWaypoints(_waypoints);
    }

    private Vector2 PointOnLeftOrRightOfScreen()
    {
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * (16f / 9f) + 1f;
        bool onTheRight = Random.value < 0.5f;
        
        if (!onTheRight)
        {
            return new Vector2(Random.Range(-screenWidth, -screenWidth * 0.5f), Random.Range(0f, screenHeight * 0.5f));
        }
        else
        {
            return new Vector2(Random.Range(screenWidth, screenWidth * 0.5f), Random.Range(0f, screenHeight * 0.5f));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector2 point in _possibleRestSpots)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}