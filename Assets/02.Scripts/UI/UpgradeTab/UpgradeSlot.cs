using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeSlot : MonoBehaviour
{
    public UpgradeTab upgradeTab;

    public TextMeshProUGUI slotLevelTxt; //Lv.(숫자)
    public TextMeshProUGUI slotValueTxt; //경우에 따라 % 붙음.
    public TextMeshProUGUI slotGoldTxt; //끝에 G 붙음.

    public int curLv = 1;
    [SerializeField] private float multiplier = 1.616f;

    public int curValue; //수동 초기화
    public int curPrice = 100;

    private void InitSlotValues()
    {

    }

    public void CalculatePrice(int mulValue)
    {
        if (mulValue == 0)
        {
            //max 계산
        }
        else
        {
            
        }
    }

    private void MultiplyPrice(int startPrice, int reachLv)
    {

    }
}
