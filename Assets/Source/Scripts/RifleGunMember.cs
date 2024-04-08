using D2D;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class RifleGunMember : SquadMember
{
    [SerializeField] private float projectileForce = 10f;
    [SerializeField] private float delayBetweenRows = 1.2f;
    [SerializeField] private int shotsRow = 4;
    [SerializeField] private PoolType bulletPrefab;

    private int shotsInRow;
    
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

        var bullet = _poolHub.Spawn(bulletPrefab, shootPoint.transform.position);
        bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

        var projectile = bullet.GetComponent<ProjectileComponent>();
        projectile.rb.velocity = Vector3.zero;

        var muzzleFlash = _poolHub.Spawn(_gameData.bulletMuzzleFlash, shootPoint.transform.position);
        muzzleFlash.transform.rotation = Quaternion.LookRotation(transform.forward);

        var direction = (currentTarget.transform.position - shootPoint.transform.position).normalized;
        projectile.rb.AddForce(direction * projectileForce, ForceMode.VelocityChange);

        projectile.enterComponent.OnEnter -= HitEnemy;
        projectile.enterComponent.OnEnter += HitEnemy;

        if (lastShotSoundTime < Time.time)
        {
            lastShotSoundTime = Time.time + _gameData.minSoundDelay;
            _audioManager.PlayOneShot(_gameData.rifleShotClip, Random.Range(0.4f, 0.5f));
        }

        shotsInRow++;

        if (shotsInRow % shotsRow == 0)
        {
            reloadTime = Time.time + memberClass.ReloadDuration;
        }
        else
        {
            reloadTime = Time.time + delayBetweenRows;
        }
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