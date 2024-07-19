using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PixelmonTab : UIBase
{
    [Header("UI")]
    [SerializeField]
    private Toggle prossessToggle;

    [SerializeField]
    private TextMeshProUGUI foodCountTxt;


    //픽셀몬 슬롯 클릭시 활성화 오브젝트
    public RectTransform clickPopUp;
    [SerializeField]
    private Button equipBtn;
    [SerializeField]
    private Button infoBtn;
    public GameObject equipProcessPanel;
    [SerializeField]
    private int choiceId;
    [SerializeField]
    private TextMeshProUGUI equipTxt;
    private string equip = "장착하기";
    private string unEquip = "해제하기";
    public TabState tabState;


    [SerializeField]
    private PixelmonPopUP infoPopUp;
    //픽셀몬 슬롯 프리팹
    [SerializeField]
    private PixelmonSlot slotPrefab;
    //전체 슬롯 부모 오브젝트 위치
    [SerializeField]
    private Transform contentTr;

    #region Info
    [Header("Info")]
    [SerializeField]
    private InventoryManager inven;
    [SerializeField]
    private PixelmonManager pixelmonManager;
    [SerializeField]
    private DataManager dataManager;
    //전체 픽셀몬 정보
    public List<PixelmonSlot> allData = new List<PixelmonSlot>();
    //미보유 픽셀몬 정보
    [SerializeField]
    private List<PixelmonSlot> noneData = new List<PixelmonSlot>();
    //보유한 픽셀몬 정보
    [SerializeField]
    private List<PixelmonSlot> possessData = new List<PixelmonSlot>();
    //편성된 픽셀몬 정보
    [SerializeField]
    private PixelmonEquipSlot[] equipData = new PixelmonEquipSlot[5];
    #endregion

    void Start()
    {
        inven = InventoryManager.Instance;
        dataManager = DataManager.Instance;
        pixelmonManager = PixelmonManager.Instance;
        InitTab();
    }

    public void InitTab()
    {
        foreach (var data in dataManager.pixelmonData.data)
        {
            PixelmonSlot slot = Instantiate(slotPrefab, contentTr);
            slot.InitSlot(this, data);
            allData.Add(slot);
        }

        for (int i = 0; i < equipData.Length; i++)
        {
            if (inven.userData.equipedPixelmons.Length > i)
            {
                equipData[i].pixelmonData = inven.userData.equipedPixelmons[i];
            }
            equipData[i].pixelmontab = this;
        }

        CheckedData();
        foodCountTxt.text = inven.userData.petFood.ToString();
    }

    public void InitInfo()
    {

    }

    public void SetPetfoodCount(int count)
    {
        inven.SetDeltaData(nameof(inven.userData.petFood), count);
        foodCountTxt.text = inven.userData.petFood.ToString();
    }

    public void CheckedData()
    {
        //findall 로 널일떄만
        possessData = allData.FindAll(obj => obj.isPossessed);
        noneData = allData.FindAll(obj => !obj.isPossessed);
        OnProssessionToggle();
    }

    public void OnProssessionToggle()
    {
        foreach (PixelmonSlot data in noneData)
        {
            data.gameObject.SetActive(!prossessToggle.isOn);
        }
    }

    public void AutoProssess()
    {

    }

    public void AutoCompose()
    {

    }

    public void Possess(int index)
    {
        allData[index].pixelmonData.isPossessed = true;
    }

    public void OnClickSlot(int index, RectTransform rectTr)
    {
        choiceId = index;
        clickPopUp.gameObject.SetActive(true);
        clickPopUp.position = rectTr.position + Vector3.down * 130;
        equipProcessPanel.gameObject.SetActive(true);
        equipProcessPanel.gameObject.transform.SetSiblingIndex(6);
        if (allData[index].pixelmonData.isEquiped)
        {
            tabState = TabState.UnEquip;
            equipTxt.text = unEquip;
        }
        else
        {
            tabState = TabState.Equip;
            equipTxt.text = equip;
        }
    }

    public void OnHideOverlay()
    {
        tabState = TabState.Normal;
        clickPopUp.gameObject.SetActive(false);
        equipProcessPanel.gameObject.SetActive(false);
    }
    
    public void OnEquip()
    {
        if(tabState == TabState.Equip) 
        {
            clickPopUp.gameObject.SetActive(false);
            equipProcessPanel.gameObject.SetActive(true);
            equipProcessPanel.gameObject.transform.SetSiblingIndex(5);
        }
        else if(tabState == TabState.UnEquip) 
        {
            for (int i = 0; i < equipData.Length; i++)
            {
                if (equipData[i].pixelmonData == allData[choiceId].pixelmonData)
                {
                    pixelmonManager.unEquipAction?.Invoke(i);
                    RemoveEquipSlot(i, choiceId);
                    break;
                }
            }
            clickPopUp.gameObject.SetActive(false);
            equipProcessPanel.gameObject.SetActive(false);
            tabState = TabState.Normal;
        }
    }


    public void EquipedPixelmon(int slotIndex)
    {
        if (equipData[slotIndex].pixelmonData != null)
        {
            equipData[slotIndex].pixelmonData.isEquiped = false;
        }
        equipData[slotIndex].Equip(allData[choiceId].pixelmonData);
        pixelmonManager.equipAction?.Invoke(slotIndex, equipData[slotIndex].pixelmonData);
        equipProcessPanel.gameObject.SetActive(false);        
        tabState = TabState.Normal;
    }

    public void RemoveEquipSlot(int slotIndex, int choiceId)
    {
        allData[choiceId].pixelmonData.isEquiped = false;
        equipData[slotIndex].UnEquip();
        equipData[slotIndex].pixelmonData = null;
    }

    public void OnInfoPopUp()
    {
        infoPopUp.gameObject.SetActive(true);
        infoPopUp.ShowPopUp(choiceId);
    }
}

public enum TabState
{
    Normal,
    Equip,
    UnEquip
}