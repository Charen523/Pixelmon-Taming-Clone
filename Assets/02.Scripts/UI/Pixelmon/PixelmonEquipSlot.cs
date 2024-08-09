using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonEquipSlot : PixelmonSlot
{
    public Player player;
    public PixelmonManager pxmManager;
    public SaveManager saveManager;
    public bool isLocked = true;
    public Image stateIcon;
    private void Awake()
    {
        saveManager = SaveManager.Instance;
    }

    private void Start()
    {
        player = Player.Instance;
        pxmManager = PixelmonManager.Instance;
        slotBtn.onClick.AddListener(OnClick);
        ChangedInfo();      
    }

    public void ChangedInfo()
    {
        slotIndex = gameObject.transform.GetSiblingIndex();
        isLocked = pxmtab.userData.isLockedSlot[slotIndex];
        if (isLocked) return;
        stateIcon.sprite = pxmManager.plusIcon;
        MyPixelmonData datas = pxmtab.allData[pxmtab.userData.equippedPxms[slotIndex].id].myPxmData;
        if (datas != null && datas.isEquipped)
        {
            myPxmData = datas;
            Equip(myPxmData);
        }
        else
            myPxmData = null;
    }

    public override void InitSlot(PixelmonTab tab, PixelmonData data)
    {
        base.InitSlot(tab, data);
    }

    public void Equip(MyPixelmonData myData)
    { 
        myPxmData = myData;
        pxmData = pxmManager.FindPixelmonData(myData.id);
        slotIcon.gameObject.SetActive(true);
        slotIcon.sprite = pxmData.icon;
        slotIconBg.sprite = pxmData.bgIcon;
        stateIcon.gameObject.SetActive(false);
        lvTxt.gameObject.SetActive(true);
        lvTxt.text = string.Format("Lv.{0}", myData.lv);
        myData.isEquipped = true;

        saveManager.userData.equippedPxms[slotIndex] = myData;
        saveManager.SetData("equippedPxms", saveManager.userData.equippedPxms);
    }

    public void UnEquip()
    {
        if(myPxmData.atvSkillId != -1)
            SkillManager.Instance.UnEquipSkill(slotIndex, myPxmData.atvSkillId);
        pxmData = null;
        myPxmData = null;
        slotIcon.gameObject.SetActive(false);
        slotIconBg.sprite = pxmManager.defaultBg;
        stateIcon.gameObject.SetActive(true);
        lvTxt.gameObject.SetActive(false);


        saveManager.userData.equippedPxms[slotIndex] = null;
        saveManager.SetData("equippedPxms", saveManager.userData.equippedPxms);
    }

    public override void OnClick()
    {
        if (isLocked) return;
        if (myPxmData == null && pxmtab.tabState != TabState.Equip)
        {
            return;
        }
        else if (myPxmData == null && pxmtab.tabState == TabState.Equip)
        {
            pxmtab.EquipedPixelmon(slotIndex);
            player.LocatedPixelmon();
        }
        else if (pxmtab.tabState == TabState.Equip)
        {
            if (myPxmData.isEquipped)
                pxmtab.UnEquipSlot(slotIndex, myPxmData.id);
            pxmtab.EquipedPixelmon(slotIndex);
            player.LocatedPixelmon();
        }
        else if(pxmtab.tabState != TabState.Equip)
        {
            base.OnClick();
        }
    }
}
