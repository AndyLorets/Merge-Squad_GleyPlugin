using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

[CreateAssetMenu(menuName = "Game/Upgrades/Skill/Spread Shot Upgrade")]
public class SpreadShotSkill : SkillUpgrades
{
    public override void Activate()
    {
        _gameData.IncreaseProjectileCount();
    }
}