using System.Numerics;
using UnityEngine;

public class OtherUpgradeSlot : UpgradeSlot
{
    [SerializeField] private float commonDiff;
    
    protected override void SetValueTxt()
    {
        slotValueTxt.text = CurValue + "%";

        if (CurLv >= maxLv)
        {
            nextValueTxt.gameObject.SetActive(false);
        }
        else
        {
            nextValueTxt.text = nextValue.ToString() + "%";
        }
    }

    protected override float ValuePerLv(int reachLv)
    {
        return reachLv * commonDiff;
    }
}