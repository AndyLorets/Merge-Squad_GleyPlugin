using D2D;
using System;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class Obstacle : Unit
{
    public Action OnDespawn;

    private void Update()
    {
        if (Vector3.Distance(transform.position, _formation.transform.position) > _gameData.ObstacleDespawnDistance)
        {
            Despawn();

            Destroy(gameObject);
        }
    }
    private void Despawn()
    {
        OnDespawn?.Invoke();
    }
}