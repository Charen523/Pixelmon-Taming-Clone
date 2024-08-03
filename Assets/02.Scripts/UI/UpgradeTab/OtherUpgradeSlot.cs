using System.Numerics;
using UnityEngine;

public class OtherUpgradeSlot : UpgradeSlot
{
    [SerializeField] private float commonDiff;
    
    protected override void SetSlotTxts()
    {
        base.SetSlotTxts();
        slotValueTxt.text = CurValue + "%";
    }

    protected override float ValuePerLv(int reachLv)
    {
        return reachLv * commonDiff;
    }
}