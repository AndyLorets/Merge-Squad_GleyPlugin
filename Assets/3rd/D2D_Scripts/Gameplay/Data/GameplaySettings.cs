using System;
using System.Collections.Generic;
using D2D.Utilities;
using D2D.Utils;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using static D2D.Utilities.SettingsFacade;
using static D2D.Utilities.CommonLazyFacade;
using static D2D.Utilities.CommonGameplayFacade;

namespace D2D.Gameplay
{
    [Serializable]
    public class Sounds
    {
        public SoundPreset onWin;
    }
    
    /// <summary>
    /// Maybe it will be better to split to more specific settings.
    /// But if it is a small game prototype maybe it is more convenient to keep it all there. 
    /// </summary>
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = "SO/GameplaySettings")]
    public class GameplaySettings : SingletonData<GameplaySettings>
    {
        [TabGroup("Common")] public List<GameObject> levels;
        [TabGroup("Common")] public Material[] groundMaterials;
        [TabGroup("Common")] public PoolType pistolProjectile;
        [TabGroup("Common")] public PoolType laserProjectile;
        [TabGroup("Common")] public float xpModifierIncrease;

        [Header("Death")]
        [TabGroup("Other")] public Color grayDeathColor;
        [TabGroup("Other")] public float grayDeathDuration;
        [TabGroup("Other")] public float grayCorpseLifetime;
        [TabGroup("Other")] public GameObject muzzleFlash;
        [TabGroup("Other")] public PoolType bulletMuzzleFlash;
        [TabGroup("Other")] public float enemyDespawnDistance = 14f;
        [TabGroup("Other")] public float neutralDespawnDistance = 14f;
        [TabGroup("Other")] public float ObstacleDespawnDistance = 20f;
        [TabGroup("Other")] public float enemyMaxSpeedLevel = 10f;

        [Header("Layers")]
        [TabGroup("Layers")] public LayerMask GroundLayer;
        [TabGroup("Layers")] public LayerMask EnemyLayer;
        [TabGroup("Layers")][Layer] public string XPLayer;

        [Header("PickUP")]
        [TabGroup("PickUp")] public float timeBeforeXPActivate;
        [TabGroup("PickUp")] public float pickUpFlyForce;

        [Header("Upgrades")]
        [TabGroup("Upgrades")] [SerializeField] private float boostInitialPricee = 150;
        [TabGroup("Upgrades")] [SerializeField] private float boostPriceStep = 75;
        [TabGroup("Upgrades")] public float speedIncreasePercent = 1.1f;
        [TabGroup("Upgrades")] public float radialWeaponDuration = 4f;
        [TabGroup("Upgrades")] public float radialWeaponCooldown = 4f;
        [TabGroup("Upgrades")] public float radialWeaponDamage = 2f;
        [TabGroup("Upgrades")] public float radialWeaponDamageCooldown = 2f;
        [TabGroup("Upgrades")] public float evolveDelay = .7f;
        [TabGroup("Upgrades")] public float[] upgradesPercentByLevel;
        [TabGroup("Upgrades")] public int maxLevelUpgrade;
        // [TabGroup("Upgrades")] public int baseIncrease;
        [TabGroup("Upgrades")] public GameObject levelUpVFX;

        private float CalculateUpgradePrice(float l)
        {
            var step = boostPriceStep;

            if (l >= 5)
                step *= 1.2f;
            
            if (l >= 7)
                step *= 1.5f;

            var result = boostInitialPricee + l * step * 2;

            if (l > 3)
                result *= 1.2f;

            if (l.Almost(1))
                result = 140;
            
            if (l.Almost(2))
                result = 250;

                // if (l >= 6)
                // result *= 1.1f;

            return result.Round();
        }

        public float PowerUpgradePrice => CalculateUpgradePrice(_db.PowerIncreaseLevel.Value);
        public float PowerNextUpgradePrice => CalculateUpgradePrice(_db.PowerIncreaseLevel.Value+1);
        
        public float FireUpgradePrice => CalculateUpgradePrice(_db.FireRateDecreaseLevel.Value);
        public float FireNextUpgradePrice => CalculateUpgradePrice(_db.FireRateDecreaseLevel.Value+1);
 
        [Header("Level Up Tween")]
        [TabGroup("Level Up Tween")] public float punchScale;
        [TabGroup("Level Up Tween")] public float punchDuration;
        [TabGroup("Level Up Tween")] public float punchDelay;

        [Header("Sounds")]
        [TabGroup("Sounds")] public AudioClip pistolShotClip;
        [TabGroup("Sounds")] public AudioClip machineGunShotClip;
        [TabGroup("Sounds")] public AudioClip rifleShotClip;
        [TabGroup("Sounds")] public AudioClip shotgunShotClip;
        [TabGroup("Sounds")] public AudioClip explosionClip;
        [TabGroup("Sounds")] public AudioClip pickUpClip;
        [TabGroup("Sounds")] public AudioClip spawnClip;
        [TabGroup("Sounds")] public AudioClip laserClip;
        [TabGroup("Sounds")] public float minSoundDelay;

        [Header("Enemies")]
        [TabGroup("Enemies")] public float baseSpeedMultiplier;
        [TabGroup("Enemies")] public float baseXPMultiplier;
        [TabGroup("Enemies")] public PoolType laserMuzzleVFX;
        [TabGroup("Enemies")] public PoolType explosionVFX;
        [TabGroup("Enemies")] public Wave[] allWaves;
        [TabGroup("Enemies")] public Wave firstWave;
        [TabGroup("Enemies"), Tag] public string enemyTag;
        [TabGroup("Enemies"), Tag] public string shieldTag;
        [TabGroup("Enemies"), Tag] public string hittableTag;

        [Header("Unlockables")]
        [TabGroup("Unlockables")] public UnlockableItem firstUnlockable;
        [TabGroup("Unlockables")] public UnlockableItem[] unlockables;

        private PoolType projectilePrefab;
        public PoolType CurrentProjectilePrefab => projectilePrefab;
        public void SetProjectilePrefab(PoolType projectile) => projectilePrefab = projectile;

        private int projectileCount;
        public int ProjectileCount => projectileCount + 1;
        public void SetSideProjectiles(int amount)
        {
            projectileCount = amount;
        }
        public void IncreaseProjectileCount() 
        {
            projectileCount = projectileCount + 2;
        }
    }
}