using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[CreateAssetMenu(menuName = "Game/Member Class")]
public class MemberClass : ScriptableObject
{
    [SerializeField]
    private float baseReloadDuration;
    [SerializeField]
    public float baseDamage;

    public LayerMask obstacleLayer;
    public LayerMask enemyLayer;

    [HideInInspector]
    public float ReloadDuration { get { return baseReloadDuration - (baseReloadDuration * (Mathf.Clamp(_db.FireRateDecreasePercent.Value + _squad.TemporaryFireRateIncrease, 0, 0.8f))); } }
    [HideInInspector]
    public float Damage { get { return baseDamage + (_db.PowerIncreasePercent.Value + _squad.TemporaryFirePowerIncrease) * baseDamage; } }
}