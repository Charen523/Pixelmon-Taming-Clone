using UnityEngine;

public class OtherUpgradeSlot : UpgradeSlot
{
   [SerializeField] private float commonDiff;

    #region UI
    protected override void SetTxt()
    {
        slotLevelTxt.text = "Lv. " + CurLv.ToString();
        slotValueTxt.text = CurValue + "%";
        SetGoldTxt();
    }
    #endregion

    #region Value
    protected override float CalculateValue(int reachLv)
    {
        return reachLv * commonDiff;
    }
    #endregion

    #region Price
    public override void CalculatePrice(int mulValue)
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

        nextValue = CalculateValue(nextLv);
        SetGoldTxt();
    }
    #endregion
}