using UnityEngine;

public class AtkUpgradeSlot : UpgradeSlot
{
    #region UI
    protected override void SetSlotTxts()
    {
        base.SetSlotTxts();
        slotValueTxt.text = CalculateTool.NumFormatter(Mathf.RoundToInt(CurValue));
    }
    #endregion

    #region Value
    protected override float ValuePerLv(int reachLv)
    {
        return reachLv;
    }
    #endregion

    #region Price
    public override void CalculatePrice(int mulValue) //next 3종 새로고침.
    {
        curUpgradeRate = mulValue;

        if (curUpgradeRate == 0)
        {
            FindMaxPrice();
        }
        else
        {
            if (CurLv + curUpgradeRate > maxLv)
            {
                curUpgradeRate = maxLv - CurLv;
            }

            nextLv =  CurLv + curUpgradeRate;
            nextValue = ValuePerLv(nextLv);
            nextPrice = CalculateTool.GetAtkTotalPrice(CurLv, nextLv);
        }
        SetGoldTxt();
    }

    protected override void FindMaxPrice()
    {
        nextLv = CurLv;
        do
        {
            nextLv++;
            nextPrice = CalculateTool.GetAtkTotalPrice(CurLv, nextLv);
        }
        while (CalculateTool.GetAtkTotalPrice(CurLv, nextLv + 1) <= ownGold && nextLv != maxLv);

        nextValue = ValuePerLv(nextLv);
        SetGoldTxt();
    }
    #endregion
}