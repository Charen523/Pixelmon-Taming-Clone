using UnityEngine;

public class AtkUpgradeSlot : UpgradeSlot
{
    protected override void SetSlotTxts()
    {
        base.SetSlotTxts();
        slotValueTxt.text = Calculater.NumFormatter(Mathf.RoundToInt(CurValue));

        Debug.Log(slotValueTxt.text);
    }
 
    protected override float ValuePerLv(int reachLv)
    {
        return reachLv;
    }
}