using System;
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
        if (skillTab.tabState == TabState.Normal && myAtvData == null)
        {
            Debug.Log("데이터가 없습니다.");
            return;
        }
        else if (skillTab.tabState == TabState.Normal && myAtvData.isEquipped)
        {
            skillTab.OnSkillPopUp(skillTab.choiceId);
        }
        else if (skillTab.tabState == TabState.Equip && SaveManager.Instance.userData.equippedPxms[slotIndex].isEquipped)
        {
            if (DataManager.Instance.activeData.data[skillTab.choiceId].isCT)
            {
                //스킬이 쿨타임 안내
                Debug.Log("해당 스킬이 쿨타임 중");
                skillTab.OnCancelEquip();
                return;
            }

            if (myAtvData != null && myAtvData.isEquipped)
                skillTab.CheckedOverlap(myAtvData.id);
            skillTab.allData[skillTab.choiceId].EquipAction();
            EquipAction(skillTab.allData[skillTab.choiceId].atvData, skillTab.allData[skillTab.choiceId].myAtvData);
            SkillManager.Instance.ExecuteSkill(Player.Instance.pixelmons[slotIndex]);
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
        myData.isEquipped = true;
        myPxmData.atvSkillId = atvData.id;
        SaveManager.Instance.userData.equippedSkills[slotIndex] = atvData.id;
        SaveManager.Instance.SetData("equippedSkills", SaveManager.Instance.userData.equippedSkills);
        SaveManager.Instance.userData.equippedPxms[slotIndex].atvSkillId = atvData.id;
        SaveManager.Instance.SetData("equippedPxms", SaveManager.Instance.userData.equippedPxms);
        SaveManager.Instance.UpdatePixelmonData(SaveManager.Instance.userData.equippedPxms[slotIndex].id, nameof(myPxmData.atvSkillId), atvData.id);
        //SkillManager.Instance.ExecuteSkill(atvData, myAtvData);
    }

    public void UnEquipAction()
    {
        myPxmData.atvSkillId = -1;
        myAtvData.isEquipped = false;
        equipIcon.sprite = null;
        atvData = null;
        myAtvData = null;
        SaveManager.Instance.userData.equippedSkills[slotIndex] = -1;
        SaveManager.Instance.SetData("equippedSkills", SaveManager.Instance.userData.equippedSkills);
        SaveManager.Instance.userData.equippedPxms[slotIndex].atvSkillId = -1;
        SaveManager.Instance.SetData("equippedPxms", SaveManager.Instance.userData.equippedPxms);
        SaveManager.Instance.UpdatePixelmonData(SaveManager.Instance.userData.equippedPxms[slotIndex].id, nameof(myPxmData.atvSkillId), myPxmData.atvSkillId);
    }
}
