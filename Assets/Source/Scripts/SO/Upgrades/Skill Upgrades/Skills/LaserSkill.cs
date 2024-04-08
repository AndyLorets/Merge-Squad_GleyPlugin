using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[CreateAssetMenu(menuName = "Game/Upgrades/Skill/Laser Upgrade")]
public class LaserSkill : SkillUpgrades
{
    public override void Activate()
    {
        _squad.SetLaserProjectile();
    }
}