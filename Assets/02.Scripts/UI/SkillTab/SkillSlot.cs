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
    public SkillTab skillTab;
    public ActiveData atvData;
    public MyAtvData myAtvData;
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
        dataManager = DataManager.Instance;
        if(slotBtn != null )
            slotBtn.onClick.AddListener(() => { OnClick(); });
    }

    public void OnClick()
    {
        skillTab.OnSkillPopUp(atvData.id);
    }

    public void EquipAction()
    {
        equipIcon.SetActive(true);
        myAtvData.isEquipped = true;
    }

    public void UnEquipAction()
    {
        myAtvData.isEquipped = false;
        equipIcon.SetActive(false);
    }

    public void InitSlot(SkillTab tab, ActiveData data)
    {
        slotIndex = data.id;
        skillTab = tab;
        atvData = data;        
        SetRankTxt();
        SetSkillIcon();
    }

    public void UpdateSlot()
    {        
        SetSkillLv();
        SetEquipTxt();       
        SetEvolveSldr();
    }

    public void OnEvolved()
    {
        while (myAtvData.isAdvancable)
        {
            skillTab.saveManager.UpdateSkillData(myAtvData.id, "isAdvancable", false);
            skillTab.saveManager.UpdatePixelmonData(myAtvData.id, "evolvedCount", myAtvData.evolvedCount - UIUtils.GetEvolveValue(myAtvData, atvData));
            skillTab.saveManager.UpdateSkillData(myAtvData.id, "lv", ++myAtvData.lv);
            SetEvolveSldr();
            SetSkillLv();
        }
    }

    public void SetRankTxt()
    {
        skillRankTxt.colorGradientPreset = GetRankColor(atvData.rank);
        skillRankTxt.text = atvData.rank;
    }
    public void SetSkillLv()
    {
        skillLv.text = myAtvData.lv.ToString();
    }

    public void SetEquipTxt()
    {
        if(myAtvData.isEquipped)
            equipIcon.gameObject.SetActive(true);
        else equipIcon.gameObject.SetActive(false);
    }

    public void SetSkillIcon()
    {
        slotIcon.sprite = atvData.icon;
        slotIconBg.sprite = atvData.bgIcon;
    }

    public void SetEvolveSldr()
    {
        int maxNum = UIUtils.GetEvolveValue(myAtvData, atvData);
        evolveSldr.maxValue = maxNum;
        evolveSldr.value = myAtvData.evolvedCount;
        evolvedCount.text = string.Format("{0}/{1}", myAtvData.evolvedCount, maxNum);
        if (myAtvData.evolvedCount >= maxNum)
        {
            skillTab.userData.ownedPxms[myAtvData.id].isAdvancable = true;
            skillTab.saveManager.UpdateSkillData(myAtvData.id, "isAdvancable", true);
        }
    }

    public TMP_ColorGradient GetRankColor(string rank)
    {
        switch (rank)
        {
            case "C":
                slotIconBg.color = skillTab.bgIconColor[0];
                return skillTab.txtColors[0];
            case "B":
                slotIconBg.color = skillTab.bgIconColor[1];
                return skillTab.txtColors[1];
            case "A":
                slotIconBg.color = skillTab.bgIconColor[2];
                return skillTab.txtColors[2];
            case "S":
                slotIconBg.color = skillTab.bgIconColor[3];
                return skillTab.txtColors[3];
            case "SS":
                slotIconBg.color = skillTab.bgIconColor[4];
                return skillTab.txtColors[4];
            default:
                return skillTab.txtColors[0];
        }
    }

}
