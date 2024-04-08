using Cinemachine;
using D2D;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class DoublePistolMember : SquadMember
{
    [SerializeField] private float projectileForce = 10f;
    [SerializeField] private PoolType bulletPrefab;
    [SerializeField] private Transform secondPoint;
    [SerializeField] private AvatarMask upperBodyMask;
    [SerializeField] private AnimationClip doubleHandsIdle;

    private bool firstPoint = false;

    public override void Init()
    {
        
    }
    public override void Shoot(Transform target)
    {
        if (reloadTime > Time.time)
        {
            return;
        }

        var bullet = _poolHub.Spawn(bulletPrefab, firstPoint ? shootPoint.transform.position : secondPoint.transform.position);
        bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

        var projectile = bullet.GetComponent<ProjectileComponent>();

        projectile.rb.velocity = Vector3.zero;

        var muzzleFlash = _poolHub.Spawn(_gameData.bulletMuzzleFlash, firstPoint ? shootPoint.transform.position : secondPoint.transform.position);
        muzzleFlash.transform.rotation = Quaternion.LookRotation(transform.forward);

        if (lastShotSoundTime < Time.time)
        {
            lastShotSoundTime = Time.time + _gameData.minSoundDelay;

            _audioManager.PlayOneShot(_gameData.pistolShotClip, .5f);
        }

        var direction = (currentTarget.transform.position - (firstPoint ? shootPoint.transform.position : secondPoint.transform.position)).normalized;
        projectile.rb.AddForce(direction * projectileForce, ForceMode.VelocityChange);

        projectile.enterComponent.OnEnter -= HitEnemy;
        projectile.enterComponent.OnEnter += HitEnemy;

        reloadTime = Time.time + memberClass.ReloadDuration;

        firstPoint = !firstPoint;
    }

    private void HitEnemy(Transform other, Transform @object)
    {
        if (other.CompareTag(_gameData.enemyTag) || other.CompareTag(_gameData.shieldTag))
        {
            other.GetComponent<IHittable>().GetHit(memberClass.Damage);
        }

        @object.gameObject.SetActive(false);
    }
    public override void SetIdleAnimation()
    {
        animancer.Layers[0].Play(doubleHandsIdle);
    }
}