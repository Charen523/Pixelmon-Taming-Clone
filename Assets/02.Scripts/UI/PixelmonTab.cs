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

    public GameObject equipOverlay;
    public GameObject addViewOverlay;
    [SerializeField]
    private int choiceId;
    [SerializeField]
    private TextMeshProUGUI equipTxt;
    private string equip = "장착하기";
    private string unEquip = "해제하기";
    public TabState tabState;

    #region 슬롯, 팝업
    [SerializeField]
    private PixelmonPopUP infoPopUp;
    //픽셀몬 슬롯 프리팹
    [SerializeField]
    private PixelmonSlot slotPrefab;
    //전체 슬롯 부모 오브젝트 위치
    [SerializeField]
    private Transform contentTr;
    #endregion

    #region 매니저
    [SerializeField]
    private PixelmonManager pixelmonManager;
    [SerializeField]
    private DataManager dataManager;
    #endregion

    #region Info
    [Header("Info")]
    [SerializeField]
    private UserData userData;
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
        userData = SaveManager.Instance.userData;
        dataManager = DataManager.Instance;
        pixelmonManager = PixelmonManager.Instance;
        InitTab();
    }

    public void InitTab()
    {
        int index = 0;
        for (int i = 0; i < dataManager.pixelmonData.data.Count; i++)
        {
            PixelmonSlot slot = Instantiate(slotPrefab, contentTr);
            slot.InitSlot(this, dataManager.pixelmonData.data[i]);
            if (userData.OwnedPxms[i] != null && slot.pxmData.id == userData.OwnedPxms[index].id)
            {
                slot.myPxmData = userData.OwnedPxms[index++];
                possessData.Add(slot);
            }
            else
            {
                noneData.Add(slot);
            }
            allData.Add(slot);
        }

        for (int i = 0; i < equipData.Length; i++)
        {
            if (userData.equippedPxms.Length > i)
            {
                equipData[i].myPxmData = userData.equippedPxms[i];
            }
            equipData[i].pxmtab = this;
        }

        CheckedData();
        foodCountTxt.text = userData.food.ToString();
    }

    public void InitInfo()
    {

    }

    public void SetPetfoodCount(int count)
    {
        //inven.SetDeltaData(nameof(inven.userData.food), count);
        //foodCountTxt.text = inven.userData.food.ToString();
    }

    public void CheckedData()
    {
        //findall 로 널일떄만
        //possessData = allData.FindAll(obj => obj.isPossessed);
        //noneData = allData.FindAll(obj => !obj.isPossessed);
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
        allData[index].myPxmData.isOwned = true;
    }

    public void OnClickSlot(int index, RectTransform rectTr)
    {
        choiceId = index;
        clickPopUp.gameObject.SetActive(true);
        addViewOverlay.gameObject.SetActive(true);
        clickPopUp.position = rectTr.position + Vector3.down * 130;
        if (allData[index].myPxmData.isEquiped)
        {
            tabState = TabState.UnEquip;
            equipTxt.text = unEquip;
        }
        else if(allData[index].myPxmData.isOwned)
        {
            tabState = TabState.Equip;
            equipTxt.text = equip;
        }
        else
        {
            tabState = TabState.Empty;
            equipTxt.text = "-";
        }
    }

    public void OnHideOverlay()
    {
        tabState = TabState.Normal;
        clickPopUp.gameObject.SetActive(false);
        addViewOverlay.gameObject.SetActive(false);
        equipOverlay.gameObject.SetActive(false);
    }
    
    public void OnEquip()
    {
        if(tabState == TabState.Equip) 
        {
            clickPopUp.gameObject.SetActive(false);
            equipOverlay.gameObject.SetActive(true);
        }
        else if(tabState == TabState.UnEquip) 
        {
            for (int i = 0; i < equipData.Length; i++)
            {
                if (equipData[i].pxmData == allData[choiceId].pxmData)
                {
                    pixelmonManager.unEquipAction?.Invoke(i);
                    RemoveEquipSlot(i, choiceId);
                    break;
                }
            }
            clickPopUp.gameObject.SetActive(false);
            tabState = TabState.Normal;
        }
    }


    public void EquipedPixelmon(int slotIndex)
    {
        equipData[slotIndex].Equip(allData[choiceId].myPxmData);
        pixelmonManager.equipAction?.Invoke(slotIndex, equipData[slotIndex].myPxmData);
        equipOverlay.gameObject.SetActive(false);        
        tabState = TabState.Normal;
    }

    public void RemoveEquipSlot(int slotIndex, int choiceId)
    {
        allData[choiceId].myPxmData.isEquiped = false;
        equipData[slotIndex].UnEquip();
        equipData[slotIndex].pxmData = null;
    }

    public void OnInfoPopUp()
    {
        infoPopUp.gameObject.SetActive(true);
        tabState = TabState.Normal;
        infoPopUp.ShowPopUp(choiceId);
    }
}

public enum TabState
{
    Normal,
    Equip,
    UnEquip,
    Empty
}