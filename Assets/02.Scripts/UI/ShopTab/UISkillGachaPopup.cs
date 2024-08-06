using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillGachaPopup : UIBase
{
    [SerializeField] private SkillGachaSlot slotPrefab;
    [SerializeField] private Transform grid;

    private const int maxSlotCnt = 30;

    private SkillGachaSlot[] slots = new SkillGachaSlot[maxSlotCnt];
    private bool isInitSlots;
    private void InitSlots()
    {
        for (int i = 0; i < maxSlotCnt; i++)
        {
            slots[i] = Instantiate(slotPrefab, grid);
            slots[i].gameObject.SetActive(false);
        }
        isInitSlots = true;
    }

    public void SetPopup(int slotCnt, ActiveData[] datas)
    {
        if (!isInitSlots)
            InitSlots();

        for (int i = 0;i < slotCnt; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].InitSlot(datas[i].bgIcon, datas[i].icon);
        }
    }
}
