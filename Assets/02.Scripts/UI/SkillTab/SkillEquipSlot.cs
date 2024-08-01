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

    private void Start()
    {
        skillTab = SkillManager.Instance.skillTab;
        ChangedInfo();
    }


    public void ChangedInfo()
    {
        slotIndex = gameObject.transform.GetSiblingIndex();
        MyAtvData myData = skillTab.equipData[slotIndex].myAtvData;
        if (myData != null && myData.isEquipped)
        {
            equipIcon.sprite = skillTab.allData[myData.id].atvData.icon;
            myAtvData = myData;
        }
    }

    public void OnClick()
    {
        if(skillTab.tabState == TabState.Normal && myAtvData.isEquipped)
            skillTab.OnSkillPopUp(atvData.id);
        else if(skillTab.tabState == TabState.Equip && SaveManager.Instance.userData.equippedPxms[slotIndex].isEquipped)
        {
            skillTab.allData[skillTab.choiceId].EquipAction();
            EquipAction(skillTab.allData[skillTab.choiceId].atvData, skillTab.allData[skillTab.choiceId].myAtvData);
            skillTab.OnCancelEquip();
        }
        else
        {
            skillTab.OnCancelEquip();
        }
    }

    public void EquipAction(ActiveData data, MyAtvData myData)
    {
        atvData = data;
        myAtvData = myData;
        equipIcon.sprite = atvData.icon;
        myPxmData.atvSkillId = atvData.id;
        SaveManager.Instance.userData.equippedSkills[slotIndex] = atvData.id;
        SaveManager.Instance.SetData("equippedSkills", SaveManager.Instance.userData.equippedSkills);
        SaveManager.Instance.userData.equippedPxms[slotIndex].atvSkillId = atvData.id;
        SaveManager.Instance.SetData("equippedPxms", SaveManager.Instance.userData.equippedPxms);
        SaveManager.Instance.UpdatePixelmonData(SaveManager.Instance.userData.equippedPxms[slotIndex].id, nameof(myPxmData.atvSkillId), atvData.id);
    }

    public void UnEquipAction()
    {
        atvData = null;
        atvData = null;
        equipIcon.sprite = null;
        myPxmData.atvSkillId = -1;
        SaveManager.Instance.userData.equippedSkills[slotIndex] = -1;
        SaveManager.Instance.SetData("equippedSkills", SaveManager.Instance.userData.equippedSkills);
        SaveManager.Instance.userData.equippedPxms[slotIndex].atvSkillId = -1;
        SaveManager.Instance.SetData("equippedPxms", SaveManager.Instance.userData.equippedPxms);
        SaveManager.Instance.UpdatePixelmonData(SaveManager.Instance.userData.equippedPxms[slotIndex].id, nameof(myPxmData.atvSkillId), myPxmData.atvSkillId);
    }
}
