using UnityEngine;

public class AtkUpgradeSlot : UpgradeSlot
{
    #region UI
    protected override void SetTxt()
    {
        slotLevelTxt.text = "Lv." + CurLv.ToString();
        slotValueTxt.text = CalculateTool.NumFormatter(Mathf.RoundToInt(CurValue * 10)); //출력할 때만 * 10 값으로: 픽셀몬 기본공격값.
        SetGoldTxt();
    }
    #endregion

    #region Value
    protected override float CalculateValue(int reachLv)
    {
        throw new System.NotImplementedException();
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
            curBtnLv = CurLv + mulValue;
            curBtnPrice = CalculateTool.GetAtkTotalPrice(CurLv, curBtnLv);
        }

        SetGoldTxt();
    }

    protected override void FindMaxPrice()
    {
        curBtnLv = CurLv;
        do
        {
            curBtnLv++;
            curBtnPrice = CalculateTool.GetAtkTotalPrice(CurLv, curBtnLv);
        }
        while (CalculateTool.GetAtkTotalPrice(CurLv, curBtnLv + 1) <= curGold);
    }
    #endregion
}