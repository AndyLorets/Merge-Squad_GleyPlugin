using UnityEngine;

public enum StatsUpgradesType
{
    AttackRate = 1,
    AttackPower = 2,
    Heal = 3
}

[CreateAssetMenu(menuName = "Game/Upgrades/Stats Upgrade")]
public class StatsUpgrades : Upgrades
{
    [SerializeField] private StatsUpgradesType statsUpgradesType;
    [SerializeField, Range(1, 100)] private float increasePercent;

    public StatsUpgradesType StatsUpgradesType => statsUpgradesType;
    public override UpgradesType GetUpgradeType() => UpgradesType.Stats;
    public float IncreasePercent => increasePercent;
}