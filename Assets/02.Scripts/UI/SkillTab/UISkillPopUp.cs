using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillPopUp : UIBase
{
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI ctTxt;
    [SerializeField] private TextMeshProUGUI descTxt;
    [SerializeField] private Button equipBtn;
    [SerializeField] private TextMeshProUGUI equipTxt;
    [SerializeField] private SkillSlot copySlot;
    [SerializeField] private SkillTab skillTab;
    private string equip = "장착하기";
    private string unEquip = "해제하기";
    private int slotIndex;
    public void ShowPopUp(SkillSlot slot, SkillTab tab)
    {
        skillTab = tab;    
        copySlot.InitSlot(tab, slot.atvData);
        copySlot.myAtvData = slot.myAtvData;
        copySlot.UpdateSlot();

        slotIndex = slot.atvData.id;
        skillNameTxt.text = slot.atvData.name;
        ctTxt.text = slot.atvData.ct.ToString();
        descTxt.text = slot.atvData.description;
        equipTxt.text = slot.myAtvData.isEquipped ? unEquip : slot.myAtvData.isOwned ? equip : "-";
    }

    public void OnEquipEvt()
    {
        skillTab.OnEquip(slotIndex);
    }
}
