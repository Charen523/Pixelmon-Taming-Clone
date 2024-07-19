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

    private void Start()
    {
        slotBtn.onClick.AddListener(OnClick);
        ChangedInfo();      
    }

    public void ChangedInfo()
    {
        PixelmonData[] datas = InventoryManager.Instance.userData.equippedPxms;
        if (datas.Length > gameObject.transform.GetSiblingIndex())
        {
            pixelmonData = datas[gameObject.transform.GetSiblingIndex()];
        }
    }

    public override void InitSlot(PixelmonTab tab, PixelmonData data)
    {
        base.InitSlot(tab, data);
    }

    public void Equip(PixelmonData data)
    {
        pixelmonData = data;
        slotIcon.sprite = data.icon;
        lvTxt.gameObject.SetActive(true);
        lvTxt.text = string.Format("Lv.{0}", pixelmonData.lv);
        data.isEquiped = true;
        plusIcon.SetActive(false);
    }

    public void UnEquip()
    {
        pixelmonData = null;
        lvTxt.gameObject.SetActive(false);
        slotIcon.sprite = null;
        plusIcon.SetActive(true);
    }

    protected override void OnClick()
    {
        base.OnClick(); 
        if (pixelmontab.tabState == TabState.Equip)
        {
            pixelmontab.EquipedPixelmon(gameObject.transform.GetSiblingIndex());
        }
    }
}
