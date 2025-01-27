using UnityEngine;

public class BackgroundBirdSpawner : Spawner
{
    [SerializeField] private int _beginningHour;
    [SerializeField] private int _endHour;

    protected override void MaybeSpawn()
    {
        if (TimeManager.Hour < _beginningHour || TimeManager.Hour >= _endHour) return;
        base.MaybeSpawn();
    }
}