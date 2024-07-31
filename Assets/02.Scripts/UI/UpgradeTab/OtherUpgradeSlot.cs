public class OtherUpgradeSlot : UpgradeSlot
{
    protected override void SetTxt()
    {
        slotLevelTxt.text = "Lv. " + CurLv.ToString();
        slotValueTxt.text = CurValue + "%";
        SetGoldTxt();
    }

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
}