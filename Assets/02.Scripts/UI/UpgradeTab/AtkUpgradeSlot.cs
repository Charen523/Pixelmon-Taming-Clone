using UnityEngine;

public class AtkUpgradeSlot : UpgradeSlot
{
    protected override void SetSlotTxts()
    {
        base.SetSlotTxts();
        slotValueTxt.text = Calculater.NumFormatter(Mathf.RoundToInt(CurValue));
    }
 
    protected override float ValuePerLv(int reachLv)
    {
        return reachLv;
    }
}