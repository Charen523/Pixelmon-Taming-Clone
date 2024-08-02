using UnityEngine;

public class OtherUpgradeSlot : UpgradeSlot
{
   [SerializeField] private float commonDiff;

    #region UI
    protected override void SetSlotTxts()
    {
        base.SetSlotTxts();
        slotValueTxt.text = CurValue + "%";
    }
    #endregion

    #region Value
    protected override float ValuePerLv(int reachLv)
    {
        return reachLv * commonDiff;
    }
    #endregion

    #region Price
    public override void CalculatePrice(int mulValue)
    {
        curUpgradeRate = mulValue;

        if (mulValue == 0)
        {
            FindMaxPrice();
        }
        else
        {
            nextLv = CurLv + mulValue;
            nextValue = ValuePerLv(nextLv);
            nextPrice = CalculateTool.GetTotalRatioPrice(CurLv, nextLv);
        }

        SetGoldTxt();
    }

    protected override void FindMaxPrice()
    {
        nextLv = CurLv;
        do
        {
            nextLv++;
            nextPrice = CalculateTool.GetTotalRatioPrice(CurLv, nextLv);
        }
        while (CalculateTool.GetTotalRatioPrice(CurLv, nextLv + 1) <= ownGold);

        nextValue = ValuePerLv(nextLv);
        SetGoldTxt();
    }
    #endregion
}