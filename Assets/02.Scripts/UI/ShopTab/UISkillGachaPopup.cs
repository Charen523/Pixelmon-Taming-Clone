using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillGachaPopup : UIBase
{
    [SerializeField] private SkillGachaSlot slotPrefab;
    [SerializeField] private Transform grid;

    private const int maxSlotCnt = 30;

    private SkillGachaSlot[] slots = new SkillGachaSlot[maxSlotCnt];

    private void Start()
    {
        for (int i = 0; i < maxSlotCnt; i++)
        {
            slots[i] = new SkillGachaSlot();
            slots[i] = Instantiate(slotPrefab, grid);
            slots[i].gameObject.SetActive(false);
        }
    }

    public void SetPopup(int slotCnt, ActiveData[] datas)
    {
        for(int i = 0;i < slotCnt; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].InitSlot(datas[i].bgIcon, datas[i].icon);
        }
    }
}
