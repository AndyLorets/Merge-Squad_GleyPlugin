using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[CreateAssetMenu(menuName = "Game/Upgrades/Skill/Double Upgrade")]
public class DoubleSkill : SkillUpgrades
{
    public override void Activate()
    {
        _squad.DoubleEveryMember();     
    }
}