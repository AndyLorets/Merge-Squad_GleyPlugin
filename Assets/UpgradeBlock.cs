using System.Collections.Generic;
using UnityEngine;

using static D2D.Utilities.CommonGameplayFacade;

public class UpgradeBlock : MonoBehaviour
{
    [SerializeField] private UpgradeItemUI itemUIPrefab;

    private Dictionary<Upgrades, UpgradeItemUI> upgrades = new Dictionary<Upgrades, UpgradeItemUI>();

    private void Awake()
    {
        _upgradesBlock = this;

        gameObject.SetActive(false);
    }

    public void CreateItemsUI(Upgrades[] upgradesList)
    {
        foreach (var upgrade in upgradesList)
        {
            var upgradeComp = Instantiate(itemUIPrefab, transform).GetComponent<UpgradeItemUI>();

            upgrades.Add(upgrade, upgradeComp);
            upgradeComp.Init(upgrade.Icon, false);
        }
    }
    public void UnlockUpgrade(Upgrades upgrade)
    {
        upgrades[upgrade].Unlock();
    }
}