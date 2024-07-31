using UnityEngine;

public class AtkUpgradeSlot : UpgradeSlot
{
    #region UI
    protected override void SetTxt()
    {
        slotLevelTxt.text = "Lv." + CurLv.ToString();
        slotValueTxt.text = CalculateTool.NumFormatter(Mathf.RoundToInt(CurValue));
        SetGoldTxt();
    }
    #endregion

    #region Value
    protected override float CalculateValue(int reachLv)
    {
        return reachLv;
    }
    #endregion

    #region Price
    public override void CalculatePrice(int mulValue) //next 3종 새로고침.
    {
        curMulValue = mulValue;

        if (mulValue == 0)
        {
            FindMaxPrice();
        }
        else
        {
            nextLv = CurLv + mulValue;
            nextValue = CalculateValue(nextLv);
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
        while (CalculateTool.GetAtkTotalPrice(CurLv, nextLv + 1) <= ownGold);

        nextValue = CalculateValue(nextLv);
        SetGoldTxt();
    }
    #endregion
}