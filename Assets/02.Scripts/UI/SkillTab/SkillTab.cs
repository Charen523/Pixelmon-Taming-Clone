using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTab : UIBase, IPointerDownHandler
{
    #region UI
    [SerializeField] private Toggle ownToggle;
    [SerializeField] private GameObject equipOverlay;
    [SerializeField] private GameObject popUpOverlay;
    #endregion

    #region 슬롯, 팝업
    [SerializeField] private UISkillPopUp infoPopUp;
    //픽셀몬 슬롯 프리팹
    [SerializeField]private SkillSlot slotPrefab;
    //전체 슬롯 부모 오브젝트 위치
    [SerializeField]private Transform contentTr;
    #endregion

    #region 매니저
    public SaveManager saveManager;
    [SerializeField]
    private SkillManager skillManager;
    [SerializeField]
    private DataManager dataManager;
    #endregion

    #region Info
    [Header("Info")]
    public UserData userData;
    //전체 스킬 정보
    public List<SkillSlot> allData = new List<SkillSlot>();
    //미보유 스킬 정보
    public List<SkillSlot> noneData = new List<SkillSlot>();
    //보유한 스킬 정보
    public List<SkillSlot> ownedData = new List<SkillSlot>();
    //편성된 스킬 정보
    public List<SkillEquipSlot> equipData = new List<SkillEquipSlot>();
    [SerializeField]
    private PixelmonLayout pxmLayout;
    #endregion

    public UnityAction<int> AddSkillAction;
    public TabState tabState = TabState.Normal;
    public Color[] bgIconColor;
    public TMP_ColorGradient[] txtColors;
    public SkillSlot choiceSlot;
    public int choiceId;

    private async void Awake()
    {
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        dataManager = DataManager.Instance;
        skillManager = SkillManager.Instance;
        SkillManager.Instance.skillTab = this;
        AddSkillAction += AddSkill;
        InitTab();
        infoPopUp = await UIManager.Show<UISkillPopUp>();
    }

    public void InitTab()
    {
        int index = 0;
         
        for (int i = 0; i < dataManager.activeData.data.Count; i++)
        {
            SkillSlot slot = Instantiate(slotPrefab, contentTr);
            slot.InitSlot(this, dataManager.activeData.data[i]);
            if (userData.ownedSkills.Count > index && userData.ownedSkills[index].id == i)
            {
                slot.myAtvData = userData.ownedSkills[index++];
                ownedData.Add(slot);
                slot.UpdateSlot();
            }
            else
            {
                noneData.Add(slot);
            }
            allData.Add(slot);
        }

        for (int i = 0; i < equipData.Count; i++)
        {
            if (userData.equippedPxms.Length > i && userData.equippedSkills[i] != -1)
            {
                equipData[i].myPxmData = userData.equippedPxms[i];
                equipData[i].myAtvData = allData[userData.equippedSkills[i]].myAtvData;
            }
            equipData[i].skillTab = this;
        }

        OnOwnedToggle();
    }

    public void OnOwnedToggle()
    {
        OnClosePopUp();
        foreach (var data in noneData)
        {
            data.gameObject.SetActive(!ownToggle.isOn);
        }
    }

    public void OnSkillPopUp(int id)
    {
        choiceId = id;
        infoPopUp.gameObject.SetActive(true);
        popUpOverlay.SetActive(true);
        infoPopUp.ShowPopUp(allData[id], this);
    }

    public void OnEquip(int id)
    {
        if (!allData[choiceId].myAtvData.isOwned)
        {
            //미보유 안내문구

            return;
        }

        if (!allData[choiceId].myAtvData.isEquipped)
        {
            tabState = TabState.Equip;
            infoPopUp.gameObject.SetActive(false);
            equipOverlay.SetActive(true);
        }
        else
        {
            CheckedOverlap(allData[id].myAtvData.id);
        }
    }


    public void OnCancelEquip()
    {
        tabState = TabState.Normal;
        infoPopUp.gameObject.SetActive(false);
        popUpOverlay.SetActive(false);
        equipOverlay.SetActive(false);
    }

    public void CheckedOverlap(int id)
    {
        foreach (var data in equipData)
        {
            if (data.myAtvData != null && data.myAtvData.id == id)
            {
                skillManager.UnEquipSkill(data.slotIndex, id);
                break;
            }
        }
            
    }

    public void AddSkill(int index)
    {
        if (!ownedData.Contains(allData[index]))
        {
            allData[index].myAtvData.isOwned = true;
            ownedData.Add(allData[index]);
            noneData.Remove(allData[index]);
        }
        allData[index].UpdateSlot();
    }

    public void OnAllLvUp()
    {
        OnClosePopUp();
        foreach (var data in ownedData)
        {
            if (data.myAtvData.isAdvancable)
            {
                data.OnEvolved();
                Debug.Log($"{data.myAtvData.id} 합성완료");
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
        pointer.position = Input.mousePosition;
#else
        pointer.position = Input.touches[0].position;
#endif
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);

        if (raycastResults.Count > 0)
        {
            if (!raycastResults[0].gameObject.TryGetComponent<SkillSlot>(out choiceSlot))
            {
                OnClosePopUp();
            }
        }
    }

    private void OnDisable()
    {
        OnClosePopUp();
    }

    public void OnClosePopUp()
    {
        if (infoPopUp != null && infoPopUp.gameObject.activeSelf)
        {
            infoPopUp.SetActive(false);
            popUpOverlay.SetActive(false);
        }
    }
}
