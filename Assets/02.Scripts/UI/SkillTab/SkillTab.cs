using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    //전체 픽셀몬 정보
    public List<SkillSlot> allData = new List<SkillSlot>();
    //미보유 픽셀몬 정보
    public List<SkillSlot> noneData = new List<SkillSlot>();
    //보유한 픽셀몬 정보
    public List<SkillSlot> ownedData = new List<SkillSlot>();
    //편성된 픽셀몬 정보
    [SerializeField]
    private List<SkillEquipSlot> equipData = new List<SkillEquipSlot>();
    [SerializeField]
    private PixelmonLayout pxmLayout;
    #endregion


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
        InitTab();
        infoPopUp = await UIManager.Show<UISkillPopUp>();
    }

    private void Start()
    {
        //userData.ownedSkills.Add(new MyAtvData());
        //saveManager.SetData(nameof(userData.ownedSkills), userData.ownedSkills);
    }
    public void InitTab()
    {
        int index = 0;
         
        for (int i = 0; i < dataManager.activeData.data.Count; i++)
        {
            SkillSlot slot = Instantiate(slotPrefab, contentTr);
            slot.InitSlot(this, dataManager.activeData.data[i]);
            if (userData.ownedSkills.Count > index && userData.ownedSkills[index].id == slot.atvData.id)
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
            if (userData.equippedPxms.Length > i && userData.equippedPxms[i].isEquipped)
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

    public void OnEquip(int slotIndex)
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
            UnEquip();
        }
    }


    public void UnEquip()
    {
        var data = equipData.Find((obj) => obj.myAtvData.id == choiceId);
        if (data != null)
        {
            allData[data.myAtvData.id].UnEquipAction();
            data.UnEquipAction();
        }
        else
        {
            //allData[pxmLayout.dataInde].UnEquipAction();
        }
    }


    public void OnCancelEquip()
    {
        tabState = TabState.Normal;
        equipOverlay.SetActive(false);
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
