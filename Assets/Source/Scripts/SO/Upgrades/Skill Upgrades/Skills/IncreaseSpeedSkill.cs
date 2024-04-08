using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[CreateAssetMenu(menuName = "Game/Upgrades/Skill/Increase Speed Upgrade")]
public class IncreaseSpeedSkill : SkillUpgrades
{
    public override void Activate()
    {
        _squad.IncreaseSpeed();
    }
}