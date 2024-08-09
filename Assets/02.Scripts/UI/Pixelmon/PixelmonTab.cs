using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    [SerializeField] TextMeshProUGUI ownSkillEffectTxt;

    public GameObject equipOverlay;
    public int choiceId;
    public TabState tabState;

    public UnityAction<int> unLockAction;
    #region 슬롯, 팝업
    [SerializeField]
    private UIPixelmonPopUp infoPopUp;
    //픽셀몬 슬롯 프리팹
    [SerializeField]
    private PixelmonSlot slotPrefab;
    //전체 슬롯 부모 오브젝트 위치
    [SerializeField]
    private Transform contentTr;
    #endregion

    #region 매니저
    public SaveManager saveManager;
    [SerializeField]
    private PixelmonManager pixelmonManager;
    [SerializeField]
    private DataManager dataManager;
    #endregion

    #region Info
    [Header("Info")]
    public UserData userData;
    //전체 픽셀몬 정보
    public List<PixelmonSlot> allData = new List<PixelmonSlot>();
    //미보유 픽셀몬 정보
    public List<PixelmonSlot> noneData = new List<PixelmonSlot>();
    //보유한 픽셀몬 정보
    public List<PixelmonSlot> ownedData = new List<PixelmonSlot>();
    //편성된 픽셀몬 정보
    public PixelmonEquipSlot[] equipData = new PixelmonEquipSlot[5];
    #endregion

    private async void Awake()
    {
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        dataManager = DataManager.Instance;
        pixelmonManager = PixelmonManager.Instance;
        pixelmonManager.pxmTab = this;
        unLockAction += GetPixelmon;
        InitTab();
        infoPopUp = await UIManager.Show<UIPixelmonPopUp>();
    }

    public void InitTab()
    {
        int index = 0;
        for (int i = 0; i < dataManager.pixelmonData.data.Count; i++)
        {
            PixelmonSlot slot = Instantiate(slotPrefab, contentTr);
            slot.InitSlot(this, dataManager.pixelmonData.data[i]);
            if (userData.ownedPxms[i] != null && userData.ownedPxms[i].isOwned)
            {
                slot.myPxmData = userData.ownedPxms[index++];
                ownedData.Add(slot);
                slot.UpdateSlot();
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
        SetfoodCount();
    }

    public void InitInfo()
    {

    }

    public void SetfoodCount()
    {
        foodCountTxt.text = userData.food.ToString();
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


    public void OnArrangeAll()
    {
        if (ownedData.Count == 0) return;

        int arrCount = 0;
        for (int i = 0; i < equipData.Length; i++)
        {
            if (!equipData[i].isLocked)
                arrCount++;
        }

        if(arrCount == 0) return;

        float[] weight = new float[ownedData.Count];
        float[] bestPxm = new float[arrCount];

        for(int i = 0; i < ownedData.Count; i++) 
        {
            weight[i] = (ownedData[i].myPxmData.atkValue + ownedData[i].myPxmData.lv * ownedData[i].pxmData.lvAtkRate) +
                ownedData[i].pxmData.traitValue;
            for (int j = 0; j < ownedData[i].myPxmData.psvSkill.Count; j++)
            {
                float aa = UIUtils.GetPsvWeight(ownedData[i].myPxmData.psvSkill[j].psvName);
                weight[i] = weight[i] + ownedData[i].myPxmData.psvSkill[j].psvValue * aa;
            }

            if (bestPxm[0] == 0 || bestPxm[0] <= weight[i])
            {
                bestPxm[0] = weight[i];
                Array.Sort(bestPxm);
            }
        }

        Array.Reverse(bestPxm);
        for(int i = 0; i < arrCount; i++) 
        {
            if (!equipData[i].isLocked && bestPxm[i] != 0)
            {
                tabState = TabState.Equip;
                choiceId = ownedData[weight.ToList().FindIndex((obj) => obj == bestPxm[i])].myPxmData.id;
                equipData[i].OnClick(); 
            }
        }
    }

    public void OnAutoEvolved()
    {
        foreach (var data in ownedData)
        {
            if (data.myPxmData.isAdvancable)
            {
                data.OnEvolved();
                Debug.Log($"{data.myPxmData.id} 합성완료");
            }
        }
    }

    public void GetPixelmon(int index)
    {
        if (!ownedData.Contains(allData[index]))
        {
            pixelmonManager.UpdatePlayerStat(
                allData[index].pxmData.basePerHp, allData[index].pxmData.basePerDef);
            allData[index].myPxmData.isOwned = true;
            ownedData.Add(allData[index]);
            noneData.Remove(allData[index]);
        }
        allData[index].UpdateSlot();
    }

    public void OnHideOverlay()
    {
        tabState = TabState.Normal;
        equipOverlay.gameObject.SetActive(false);
        infoPopUp.gameObject.SetActive(false);
    }
    
    public void OnEquip()
    {
        if (tabState == TabState.Equip) 
        {
            equipOverlay.gameObject.SetActive(true);
        }
        else if(tabState == TabState.UnEquip) 
        {
            for (int i = 0; i < equipData.Length; i++)
            {
                if (equipData[i].pxmData == allData[choiceId].pxmData)
                {
                    UnEquipSlot(i, choiceId);
                    break;
                }
            }
            tabState = TabState.Normal;
        }
    }


    public void EquipedPixelmon(int slotIndex)
    {
        equipData[slotIndex].Equip(allData[choiceId].myPxmData);
        allData[choiceId].equipIcon.SetActive(true);
        pixelmonManager.equipAction?.Invoke(slotIndex, equipData[slotIndex].myPxmData);
        equipOverlay.gameObject.SetActive(false);        
        tabState = TabState.Normal;
    }

    public void UnEquipSlot(int slotIndex, int choiceId)
    {
        pixelmonManager.unEquipAction?.Invoke(slotIndex);
        allData[choiceId].myPxmData.isEquipped = false;
        allData[choiceId].equipIcon.SetActive(false);
        equipData[slotIndex].UnEquip();
    }

    public void OnInfoPopUp(int id)
    {
        choiceId = id;
        infoPopUp.gameObject.SetActive(true);
        infoPopUp.ShowPopUp(choiceId, this);
    }
}

