using D2D;
using D2D.Utilities;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class PistolMember : SquadMember
{
    [SerializeField] private float projectileForce = 10f;

    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var rightVector = transform.right / 2;

        for (int i = 0; i < _gameData.ProjectileCount; i++)
        {
            var bullet = _poolHub.Spawn(_gameData.CurrentProjectilePrefab, shootPoint.transform.position);
            bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

            var projectile = bullet.GetComponent<ProjectileComponent>();

            projectile.SetColor(memberColor);

            projectile.rb.velocity = Vector3.zero;

            var muzzleFlash = _poolHub.Spawn(_gameData.bulletMuzzleFlash, shootPoint.transform.position);
            muzzleFlash.transform.rotation = Quaternion.LookRotation(transform.forward);

            var direction = (currentTarget.transform.position - shootPoint.transform.position).normalized;

            Vector3 sideDir;
            
            if ((_gameData.ProjectileCount - 1) / 2 == i)
            {
                sideDir = Vector3.zero;
            }
            else
            {
                sideDir = Vector3.Lerp(-rightVector, rightVector, (i + 1f) / _gameData.ProjectileCount);

            }
            
            projectile.rb.AddForce((direction + sideDir) * projectileForce, ForceMode.VelocityChange);

            projectile.enterComponent.OnEnter -= HitEnemy;
            projectile.enterComponent.OnEnter += HitEnemy;
        }

        if (lastShotSoundTime < Time.time)
        {
            lastShotSoundTime = Time.time + _gameData.minSoundDelay;
            _audioManager.PlayOneShot(_gameData.pistolShotClip, .5f);
        }

        var reload = memberClass.ReloadDuration - memberClass.ReloadDuration * (1 + EvolveLevel / 20) / 1.5f;

        reloadTime = Time.time + memberClass.ReloadDuration;
    }

    private void HitEnemy(Transform other, Transform @object)
    {
        if (other.CompareTag(_gameData.hittableTag))
        {
            other.GetComponent<IHittable>().GetHit(memberClass.Damage + EvolveLevel);
        }

        @object.gameObject.SetActive(false);
    }
}