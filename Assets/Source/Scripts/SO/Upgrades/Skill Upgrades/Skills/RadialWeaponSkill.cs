using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[CreateAssetMenu(menuName = "Game/Upgrades/Skill/Radial Weapon Upgrade")]
public class RadialWeaponSkill : SkillUpgrades
{
    public override void Activate()
    {
        _radialWeapon.AddWeapon();
    }
}