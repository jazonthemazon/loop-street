using UnityEngine;

public class AlterationSpawner : Spawner
{
    [Header("Spawnrate Alteration")]
    [SerializeField] private float _spawnrateAlteration;
    [SerializeField] private int _beginningHour;
    [SerializeField] private int _endHour;

    protected override void OnEnable()
    {
        base.OnEnable();
        Actions.OnHourChanged += MaybeAlterSpawnRate;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Actions.OnHourChanged -= MaybeAlterSpawnRate;
    }

    private void MaybeAlterSpawnRate()
    {
        if (TimeManager.Hour == _beginningHour)
        {
            SpawnProbability *= _spawnrateAlteration;
        }
        else if (TimeManager.Hour == _endHour)
        {
            SpawnProbability *= 1 / _spawnrateAlteration;
        }
    }
}