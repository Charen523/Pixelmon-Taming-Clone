using UnityEngine;

public class UpgradeTab : UIBase
{
    [SerializeField] private UpgradeSlot[] upgradeSlots;
    private int[] upgradeLvs => SaveManager.Instance.userData.UpgradeLvs;
    
    private void Awake()
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].upgradeTab = this;
            upgradeSlots[i].CurLv = upgradeLvs[i];
        }
    }

    public void CurrentToggle(int toggleIndex)
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].CalculatePrice(toggleIndex);
        }
    }

    public void SaveUpgradeLvs(int index, int curLv)
    {
        int[] newUpgradeLvs = upgradeLvs;
        newUpgradeLvs[index] = curLv;
        SaveManager.Instance.SetData(nameof(upgradeLvs), newUpgradeLvs);
    }
}