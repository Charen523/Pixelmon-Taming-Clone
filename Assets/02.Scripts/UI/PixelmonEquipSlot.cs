using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PixelmonEquipSlot : PixelmonSlot
{
    public GameObject lockIcon;
    public bool isLocked = true;

    private void Start()
    {
        slotBtn.onClick.AddListener(OnClick);
        ChangedInfo();      
    }

    public void ChangedInfo()
    {
        PixelmonData[] datas = InventoryManager.Instance.userData.equipedPixelmons;
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
        data.isEquiped = true;
       
    }

    public void UnEquip()
    {
        pixelmonData = null;
        slotIcon.sprite = null;
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
