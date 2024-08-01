using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquipSlot : MonoBehaviour
{
    public SkillTab skillTab;
    public ActiveData atvData;
    public MyAtvData myAtvData;
    public MyPixelmonData myPxmData;
    public Image equipIcon;
    public int slotIndex;
    public void OnClick()
    {
        if(skillTab.tabState == TabState.Normal && myAtvData != null)
            skillTab.OnSkillPopUp(atvData.id);
        else if(skillTab.tabState == TabState.Equip && SaveManager.Instance.userData.equippedPxms[slotIndex].isEquipped)
        {
            skillTab.allData[skillTab.choiceId].EquipAction();
            EquipAction(skillTab.allData[skillTab.choiceId]);
        }
    }

    public void EquipAction(SkillSlot slot)
    {
        atvData = slot.atvData;
        myAtvData = slot.myAtvData;
        equipIcon.sprite = atvData.icon;
        myPxmData.atvSkillId = atvData.id;
    }

    public void UnEquipAction()
    {
        atvData = null;
        myAtvData = null;
        equipIcon.sprite = null;
        myPxmData.atvSkillId = null;
    }
}
