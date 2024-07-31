using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTab : UIBase
{
    #region UI
    [SerializeField] private Toggle ownToggle;
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
    private SkillEquipSlot[] equipData = new SkillEquipSlot[5];
    #endregion

    public Color[] rankColor;
    private async void Awake()
    {
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        dataManager = DataManager.Instance;
        skillManager = SkillManager.Instance;
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
            if (userData.ownedSkills.Count != 0 && userData.ownedSkills[i].isOwned)
            {
                slot.myData = userData.ownedSkills[index++];
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
            if (userData.equippedPxms.Length > i && userData.equippedPxms[i].isEquipped)
            {
                equipData[i].myPxmData = userData.equippedPxms[i];
                equipData[i].myAtvData = allData[userData.equippedSkills[i]].myData;
            }
            equipData[i].skillTab = this;
        }

        OnOwnedToggle();
    }

    public void OnOwnedToggle()
    {
        foreach (var data in noneData)
        {
            data.gameObject.SetActive(!ownToggle.isOn);
        }
    }

    public void OnSkillPopUp()
    {
        infoPopUp.gameObject.SetActive(true);
    }

    public void OnEquip()
    {

    }

    public void OnAllLvUp()
    {
        foreach (var data in ownedData)
        {
            if (data.myData.isAdvancable)
            {
                data.OnEvolved();
                Debug.Log($"{data.myData.id} 합성완료");
            }
        }
    }
}
