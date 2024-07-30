using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTab : UIBase
{
    private PixelmonStatus upgradeStat;

    private int currentGold => SaveManager.Instance.userData.gold;

    #region UI
    [SerializeField] private UpgradeSlot[] upgradeSlots;


    #endregion

    #region Tab Fields & Properties

    private int _mulValue;
    private int MulValue
    {
        get
        {
            return _mulValue;
        }
        set
        {
            if (_mulValue != value)
            {
                _mulValue = value;
            }
        }
    }

    public int[] UpgradeLvs
    {
        get
        {
            return SaveManager.Instance.userData.UpgradeLvs;
        }

        set
        {
            SaveManager.Instance.SetData(nameof(SaveManager.Instance.userData.UpgradeLvs), value);
        }
    }
    public int this[int index]
    {
        get
        {
            return UpgradeLvs[index];
        }
        set
        {
            UpgradeLvs[index] = value;
        }
    }
    #endregion

    private void Awake()
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].upgradeTab = this;
        }
    }

    private void Start()
    {
        upgradeStat = PixelmonManager.Instance.upgradeStatus;
    }


    public void CurrentToggle(int toggleIndex)
    {
        MulValue = toggleIndex;
    }
}
