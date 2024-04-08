using UnityEngine;

[CreateAssetMenu(menuName = "Game/Upgrades/Skill Upgrade")]
public class SkillUpgrades : Upgrades
{
    public override UpgradesType GetUpgradeType() => UpgradesType.Skill;
    public virtual void Activate()
    {
    }
}