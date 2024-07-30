using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTab : UIBase
{
    private PixelmonStatus upgradeStat;

    #region UI
    [SerializeField] private UpgradeSlot[] upgradeSlots;

    private int mulValue;
    #endregion

    private void Start()
    {
        upgradeStat = PixelmonManager.Instance.upgradeStatus;
    }


    public void CurrentToggle(int toggleIndex)
    {
        mulValue = toggleIndex;
    }
}
