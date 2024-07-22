using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonEquipSlot : PixelmonSlot
{
    public GameObject plusIcon;
    public GameObject lockIcon;
    public bool isLocked = true;
    public Player player;
    public PixelmonManager pxmManager;
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
        MyPixelmonData datas = pxmtab.userData.equippedPxms[slotIndex];
        if (datas != null && datas.isEquiped)
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
        pxmData = pxmManager.FindPixelmonData(myData);
        slotIcon.sprite = pxmData.icon;
        lvTxt.gameObject.SetActive(true);
        lvTxt.text = string.Format("Lv.{0}", myData.lv);
        myData.isEquiped = true;
        plusIcon.SetActive(false);
    }

    public void UnEquip()
    {
        pxmData = null;
        lvTxt.gameObject.SetActive(false);
        slotIcon.sprite = null;
        plusIcon.SetActive(true);
    }

    protected override void OnClick()
    {
        if (myPxmData == null && pxmtab.tabState != TabState.Equip)
        {
            return;
        }
        else if (myPxmData == null && pxmtab.tabState == TabState.Equip)
        {
            pxmtab.EquipedPixelmon(slotIndex);
        }
        else if (pxmtab.tabState == TabState.Equip)
        {
            if (myPxmData.isEquiped)
                pxmtab.RemoveEquipSlot(slotIndex, myPxmData.id);
            pxmtab.EquipedPixelmon(slotIndex);
        }
        else if(pxmtab.tabState != TabState.Equip)
        {
            pxmtab.tabState = TabState.UnEquip;
            pxmtab.OnClickSlot(pxmData.id, rectTr);
        }

    }
}
