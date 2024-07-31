using UnityEngine;

public class AtkUpgradeSlot : UpgradeSlot
{
    protected override void SetTxt()
    {
        slotLevelTxt.text = "Lv." + CurLv.ToString();
        slotValueTxt.text = NumberFormatter.FormatIntNum(CurValue * 10); //txt만 *10값.
        SetGold();
    }
}