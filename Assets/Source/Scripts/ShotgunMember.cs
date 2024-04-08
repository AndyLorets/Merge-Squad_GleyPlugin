using D2D;
using D2D.Utilities;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class ShotgunMember : SquadMember
{
    [SerializeField] private Vector2 projectileForce;
    [SerializeField] private int shotParts = 5;
    [SerializeField] private PoolType bulletPrefab;
    
    public override void Init()
    {
        runForward = animations.RunWithRifle;
    }
    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var rightVector = transform.right / 2;

        for (int i = 0; i < shotParts; i++)
        {
            var bullet = _poolHub.Spawn(bulletPrefab, shootPoint.transform.position);
            bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

            var projectile = bullet.GetComponent<ProjectileComponent>();

            projectile.rb.velocity = Vector3.zero;

            var muzzleFlash = _poolHub.Spawn(_gameData.bulletMuzzleFlash, shootPoint.transform.position);
            muzzleFlash.transform.rotation = Quaternion.LookRotation(transform.forward);

            var direction = (currentTarget.transform.position - shootPoint.transform.position).normalized;

            var randomDir = Vector3.Lerp(-rightVector, rightVector, Random.value);

            projectile.rb.AddForce((direction + randomDir)* projectileForce.RandomFloat(), ForceMode.VelocityChange);

            projectile.enterComponent.OnEnter -= HitEnemy;
            projectile.enterComponent.OnEnter += HitEnemy;
        }

        if (lastShotSoundTime < Time.time)
        {
            lastShotSoundTime = Time.time + _gameData.minSoundDelay;
            _audioManager.PlayOneShot(_gameData.shotgunShotClip, Random.Range(0.4f, 0.5f));
        }

        reloadTime = Time.time + memberClass.ReloadDuration;
    }

    private void HitEnemy(Transform other, Transform @object)
    {
        if (other.CompareTag(_gameData.hittableTag))
        {
            other.GetComponent<IHittable>().GetHit(memberClass.Damage);
        }

        @object.gameObject.SetActive(false);
    }
}