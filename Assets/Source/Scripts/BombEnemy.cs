using D2D;
using D2D.Gameplay;
using D2D.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using static D2D.Utilities.CommonGameplayFacade;

public class BombEnemy : EnemyComponent
{
    [SerializeField] private SexyOverlap bombOverlap;
 
    public override void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Time.frameCount % refreshNavMeshFrames == 0)
        {
            navMesh.SetDestination(_formation.transform.position);

            if (Vector3.Distance(transform.position, _formation.transform.position) > _gameData.enemyDespawnDistance)
            {
                DespawnEnemy();
            }
        }

        if (overlap.HasTouch && attackTimer <= Time.time)
        {
            Explode();
        }
    }
    internal override void Die()
    {
        var powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity).Get<XPPoint>();
        powerUp.Init(transform.position, _formation.transform.position);

        _db.Money.Value += deathReward;

        Explode();
    }
    private void Explode()
    {
        isDead = true;

        navMesh.isStopped = true;
        navMesh.ResetPath();
        navMesh.velocity = Vector3.zero;
        navMesh.enabled = false;

        canvas.HealthBar.gameObject.SetActive(false);

        overlap.enabled = false;

        capsCollider.enabled = false;

        DHaptic.HapticLight();

        _enemySpawn.EnemyDied();

        foreach (var touched in bombOverlap.AllTouched)
        {
            if (capsCollider != touched)
            {
                touched.Get<Health>().ApplyDamage(gameObject, damage);
            }
        }

        _poolHub.Spawn(_gameData.explosionVFX, transform.position);

        Destroy(gameObject);
    }
}