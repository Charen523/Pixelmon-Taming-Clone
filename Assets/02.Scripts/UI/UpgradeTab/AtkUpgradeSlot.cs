using UnityEngine;

public class AtkUpgradeSlot : UpgradeSlot
{
    protected override void SetTxt()
    {
        slotLevelTxt.text = "Lv." + CurLv.ToString();
        slotValueTxt.text = CalculateTool.NumFormatter(Mathf.RoundToInt(CurValue * 10)); //출력할 때만 * 10 값으로: 픽셀몬 기본공격값.
        SetGoldTxt();
    }
}