using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : Spawner
{
    protected override void Spawn()
    {
        List<Vector2> waypoints = new List<Vector2>();

        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * (16f / 9f) + 1f;

        waypoints.Add(new Vector2(Random.Range(-screenWidth, -screenWidth * 0.5f), Random.Range(0f, screenHeight * 0.5f)));
        waypoints.Add(new Vector2(Random.Range(screenWidth, screenWidth * 0.5f), Random.Range(0f, screenHeight * 0.5f)));

        Spawnable spawnable = Instantiate(_spawnablePrefab, waypoints[0], Quaternion.identity);
        spawnable.SetWaypoints(waypoints);
    }
}