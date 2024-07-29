using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class UIPixelmonPopUp : UIBase
{
    #region Data
    [SerializeField]
    private PixelmonTab pxmTab;
    [SerializeField]
    private PixelmonData data;
    [SerializeField]
    private MyPixelmonData myData;
    [SerializeField]
    private SaveManager saveManager;
    #endregion

    #region UI
    [Header("UI")]
    [SerializeField] private GameObject overlay;
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private Image rankIcon;
    [SerializeField] private Sprite[] rankSprites;
    [SerializeField] private TextMeshProUGUI pxmNameTxt;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private Image pxmIcon;
    [SerializeField] private TextMeshProUGUI lvTxt;


    [SerializeField] private Button equipBtn;
    [SerializeField] private TextMeshProUGUI equipTxt;
    [SerializeField] private Button feedingBtn;
    [SerializeField] private GameObject feedingArrow;
    [SerializeField] private Button evolvedBtn;
    [SerializeField] private GameObject evolvedArrow;

    [SerializeField] private TextMeshProUGUI atkValue;
    [SerializeField] private TextMeshProUGUI traitType;
    [SerializeField] private TextMeshProUGUI traitValue;
    [SerializeField] private TextMeshProUGUI[] psvRank;
    [SerializeField] private TextMeshProUGUI[] psvName;
    [SerializeField] private TextMeshProUGUI[] psvValue;
    [SerializeField] private TextMeshProUGUI[] ownEffectValue;

    [SerializeField] private TextMeshProUGUI infoBoxTxt;
    [SerializeField] private TextMeshProUGUI feedCount;
    [SerializeField] private Button[] infoBtns;
    #endregion

    private string equip = "장착하기";
    private string unEquip = "해제하기";
    public int infoIndex = 0;
    public int foodPerExp = 10;
    public int maxExp = 50;

    private void Start()
    {
        saveManager = SaveManager.Instance;
    }

    public void ShowPopUp(int dataIndex, PixelmonTab tab)
    {
        pxmTab = tab;
        infoIndex = dataIndex;
        if (infoIndex == 0)
            infoBtns[0].gameObject.SetActive(false);
        else if (infoIndex == pxmTab.allData.Count)
            infoBtns[1].gameObject.SetActive(false);
        SetData();
    }

    public void OnPreviousInfo()
    {
        infoIndex--;
        SetData();
        if (infoIndex == 0)
        {
            infoBtns[0].gameObject.SetActive(false);
        }
        else if (infoIndex == pxmTab.allData.Count - 2)
        {
            infoBtns[1].gameObject.SetActive(true);
        }
    }
    public void OnNextInfo()
    {
        infoIndex++;
        SetData();
        if (infoIndex == 1)
        {
            infoBtns[0].gameObject.SetActive(true);
        }
        else if (infoIndex == pxmTab.allData.Count - 1)
        {
            infoBtns[1].gameObject.SetActive(false);     
        }
    }

    private void SetData()
    {
        data = pxmTab.allData[infoIndex].pxmData;
        myData = pxmTab.allData[infoIndex].myPxmData;
        pxmTab.choiceId = myData.id;
        equipTxt.text = myData.isEquipped ? unEquip : equip;
        InitInfo();
    }

    private void InitInfo()
    {
        var rank = (PixelmonRank)SetRankSprite(data.rank);
        rankTxt.text = rank.TranslateRank();
        rankIcon.sprite = rankSprites[SetRankSprite(data.rank)];
        pxmNameTxt.text = data.name;
        pxmIcon.sprite = data.icon;
        lvTxt.text = string.Format($"{myData.lv}/50");
        SetStars();
        SetEquippedEffect();
        SetOwnedEffect();
    }

    public void OnEquip()
    {
        if (!myData.isEquipped)
        {
            pxmTab.tabState = TabState.Equip;
            pxmTab.OnEquip();     
        }
        else
        {
            pxmTab.tabState = TabState.UnEquip;
            pxmTab.OnEquip();
        }
        gameObject.SetActive(false);
    }

    public void OnFeeding()
    {
        if (saveManager.userData.food >= myData.maxExp)
        {
            saveManager.SetDeltaData("food", saveManager.userData.food - myData.maxExp);
            pxmTab.SetfoodCount();
            saveManager.UpdatePixelmonData(myData.id, "lv", ++myData.lv);
            lvTxt.text = string.Format($"{myData.lv}/50");
            pxmTab.allData[myData.id].SetPxmLv();
            Debug.Log("레벨 업!");
        }
        else
        {
            Debug.Log("남은 먹이가 없습니다.");
        }
    }

    public void OnEvovled()
    {
        pxmTab.allData[myData.id].OnEvolved();
        SetStars();
        Debug.Log("합성완료(팝업)");
    }


    public void SetStars()
    {
        for (int i = 0; i <= myData.star; i++)
        {
            if (i < myData.star)
                stars[i].SetActive(true);
            else
                stars[i].SetActive(false);
        }
    }

    public int SetRankSprite(string rank)
    {
        switch (rank)
        {
            case "Common":
                return 0;
            case "Advanced":
                return 1;
            case "Rare":
                return 2;
            case "Epic":
                return 3;
            case "Legendary":
                return 4;
            case "Unique":
                return 5;
            default:
                return 0;
        }
    }

    public void SetEquippedEffect()
    {
        atkValue.text = myData.atkValue.ToString();
        traitType.text = myData.traitType;
        traitValue.text = myData.psvSkill.ToString();
        for (int i = 0; i < psvName.Length; i++)
        {
            if (psvName == null) break;
            psvRank[i].text = myData.psvSkill[i].psvRank;
            psvName[i].text = myData.psvSkill[i].psvName;
            psvValue[i].text = myData.psvSkill[i].psvValue.ToString();
        }
    }

    public void SetOwnedEffect()
    {
        for(int i = 0; i < ownEffectValue.Length; i++) 
            ownEffectValue[i].text = myData.ownEffectValue[i].ToString();
    }
}