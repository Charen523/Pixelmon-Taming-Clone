using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTab : UIBase
{
    private PixelmonStatus upgradeStat;

    #region UI
    [SerializeField] private UpgradeSlot[] upgradeSlots;
    #endregion

    private void Start()
    {
        upgradeStat = PixelmonManager.Instance.upgradeStatus;
    }
}
