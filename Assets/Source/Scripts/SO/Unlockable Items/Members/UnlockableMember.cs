using UnityEngine;

[CreateAssetMenu(menuName = "Unlockable/Member")]
public class UnlockableMember : UnlockableItem
{
    [SerializeField] private MemberUpgrades memberUpgradesSO;
    [SerializeField] private UnlockableMember nextUnlockable;

    public MemberUpgrades MemberUpgradesSO => memberUpgradesSO;
    public UnlockableMember NextUnlockable => nextUnlockable;
}