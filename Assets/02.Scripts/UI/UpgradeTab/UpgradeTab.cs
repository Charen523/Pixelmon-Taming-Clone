using System;
using UnityEngine;

public class UpgradeTab : UIBase
{
    private PixelmonStatus upgradeStat;

    private int currentGold => SaveManager.Instance.userData.gold;

    #region UI
    [SerializeField] private UpgradeSlot[] upgradeSlots;
    #endregion

    #region Tab Fields & Properties

    private int mulValue;

    private int[] upgradeLvs => SaveManager.Instance.userData.UpgradeLvs;
    
    #endregion

    private void Awake()
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].upgradeTab = this;
            upgradeSlots[i].slotIndex = (UpgradeIndex)i;
            upgradeSlots[i].CurLv = upgradeLvs[i];
        }
    }

    private void Start()
    {
        upgradeStat = PixelmonManager.Instance.upgradeStatus;
    }

    public void CurrentToggle(int toggleIndex)
    {
        mulValue = toggleIndex;

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].CalculatePrice(mulValue);
        }
    }
}
