using UnityEngine;

public enum UpgradesType
{
    Stats = 1,
    Member = 2,
    Skill = 3
}

public class Upgrades : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string upgradeText;

    public virtual UpgradesType GetUpgradeType() => UpgradesType.Stats;
    public Sprite Icon => icon;
    public string UpgradeText => upgradeText;
}