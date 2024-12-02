using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class PedestrianSpawner : MonoBehaviour
{
    [SerializeField] GameObject _pedestrianPrefab;
    [SerializeField] int _cooldown;
    [SerializeField] Vector2 _positionOne;

    private int currentCooldown = 0;

    void Update()
    {
        if (currentCooldown == 0)
        {
            int randomNumber = Random.Range(1, 101);
            if (randomNumber <= 1)
            {
                GameObject pedestrian = Instantiate(_pedestrianPrefab, transform.position, Quaternion.identity);
                pedestrian.GetComponent<PedestrianBehaviour>().SetSize(Random.Range(1.5f, 2.2f));
                currentCooldown = _cooldown;
                pedestrian.transform.DOMove(_positionOne, 2f).SetEase(Ease.Linear);
            }
        }
        currentCooldown = Mathf.Max(currentCooldown - 1, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.3f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_positionOne, 0.3f);
    }
}
