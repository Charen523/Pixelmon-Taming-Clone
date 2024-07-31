using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.U2D.Aseprite;
using System;
using System.Xml.Linq;

public class SkillSlot : MonoBehaviour
{
    SkillTab skillTab;
    public ActiveData data;
    public MyAtvData myData;
    DataManager dataManager;
    #region 슬롯정보
    [SerializeField] protected int slotIndex;
    [SerializeField]
    protected RectTransform rectTr;
    [SerializeField]
    protected Button slotBtn;
    [SerializeField]
    protected Image slotIcon;
    [SerializeField]
    protected Image slotIconBg;

    #endregion

    #region UI
    public TextMeshProUGUI skillRankTxt;
    [SerializeField] private GameObject equipIcon;
    [SerializeField] protected TextMeshProUGUI skillLv;
    [SerializeField] private Slider evolveSldr;
    [SerializeField] private TextMeshProUGUI evolvedCount;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void InitSlot(SkillTab tab, ActiveData atvData)
    {
        dataManager = DataManager.Instance;
        skillTab = tab;
        data = atvData;
        slotBtn.onClick.AddListener(() => { skillTab.OnSkillPopUp(); });
        
    }

    public void UpdateSlot()
    {
        myData = SaveManager.Instance.userData.ownedSkills[myData.id];
        SetRankTxt();
        SetSkillLv();
        SetEquipTxt();
        SetSkillIcon();
        SetEvolveSldr();
        SetEvolveSldr();
    }

    public void OnSlotClick()
    {
        skillTab.OnSkillPopUp();
    }


    public void OnEvolved()
    {
        skillTab.saveManager.UpdatePixelmonData(myData.id, "isAdvancable", false);
        //skillTab.saveManager.UpdatePixelmonData(myData.id, "evolvedCount", myData.evolvedCount - UIUtils.GetEvolveValue(myData, data));
        skillTab.saveManager.UpdatePixelmonData(myData.id, "star", ++myData.lv) ;
    }

    public void SetRankTxt()
    {
        skillRankTxt.text = data.rank;
        skillRankTxt.colorGradientPreset.topRight = GetRankColor(data.rank);
        skillRankTxt.colorGradientPreset.bottomLeft = GetRankColor(data.rank);
    }
    public void SetSkillLv()
    {
        skillLv.text = myData.lv.ToString();
    }

    public void SetEquipTxt()
    {
        if(myData.isEquipped)
            equipIcon.gameObject.SetActive(true);
        else equipIcon.gameObject.SetActive(false);
    }

    public void SetSkillIcon()
    {
        slotIcon.sprite = data.icon;
        slotIconBg.sprite = data.bgIcon;
    }

    public void SetEvolveSldr()
    {
        int maxNum = UIUtils.GetEvolveValue(myData, data);
        evolveSldr.maxValue = maxNum;
        evolveSldr.value = myData.evolvedCount;
        evolvedCount.text = string.Format("{0}/{1}", myData.evolvedCount, maxNum);
        if (myData.evolvedCount >= maxNum)
        {
            skillTab.userData.ownedPxms[myData.id].isAdvancable = true;
            skillTab.saveManager.UpdateSkillData(myData.id, "isAdvancable", true);
        }
    }



    public Color GetRankColor(string rank)
    {
        switch (rank)
        {
            case "C":
                return skillTab.rankColor[0];
            case "B":
                return skillTab.rankColor[1];
            case "A":
                return skillTab.rankColor[2];
            case "S":
                return skillTab.rankColor[3];
            case "SS":
                return skillTab.rankColor[4];
            default:
                return Color.white;
        }
    }

}
