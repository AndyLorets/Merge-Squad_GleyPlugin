using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[CreateAssetMenu(menuName = "Game/Upgrades/Skill/XP Increase Upgrade")]
public class ExperienceIncreaseSkill : SkillUpgrades
{
    public override void Activate()
    {
        _xpPicker.IncreaseXPModifier();
    }
}