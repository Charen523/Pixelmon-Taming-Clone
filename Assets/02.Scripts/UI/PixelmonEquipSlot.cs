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
        MyPixelmonData[] datas = SaveManager.Instance.userData.equippedPxms;
        if (datas.Length > gameObject.transform.GetSiblingIndex())
        {
            myPxmData = datas[gameObject.transform.GetSiblingIndex()];
        }
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
        if (pxmtab.tabState == TabState.Normal && pxmData != null)
        {
            pxmtab.OnClickSlot(pxmData.id, rectTr);
        }
        else if (pxmtab.tabState == TabState.Equip)
        {
            pxmtab.EquipedPixelmon(gameObject.transform.GetSiblingIndex());
        }
    }
}
