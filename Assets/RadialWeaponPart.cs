using D2D;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class RadialWeaponPart : Unit
{
    private float timer;

    private void OnTriggerStay(Collider other)
    {
        if (Time.time >= timer)
        {
            if (other.CompareTag(_gameData.hittableTag))
            {
                var enemies = Physics.OverlapSphere(transform.position, 1, _gameData.EnemyLayer);

                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<IHittable>().GetHit(_gameData.radialWeaponDamage);
                }

                timer = Time.time + _gameData.radialWeaponDamageCooldown;
            }

        }
    }
}