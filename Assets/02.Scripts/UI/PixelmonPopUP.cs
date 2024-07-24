using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonPopUP : UIBase
{
    [SerializeField]
    private PixelmonTab pxmTab;
    [SerializeField]
    private PixelmonData data;
    [SerializeField]
    private MyPixelmonData myData;

    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI rankTxt;
    [SerializeField]
    private Image rankIcon;
    [SerializeField]
    private Sprite[] rankSprites;
    [SerializeField]
    private TextMeshProUGUI pxmNameTxt;
    [SerializeField]
    private GameObject[] stars;
    [SerializeField]
    private Image pxmIcon;
    [SerializeField]
    private TextMeshProUGUI lvTxt;

    [SerializeField]
    private TextMeshProUGUI infoBoxTxt;
    [SerializeField]
    private TextMeshProUGUI feedCount;

    [SerializeField]
    private Button[] infoBtns;
    public int infoIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        InitInfo();
    }


    private void InitInfo()
    {
        rankTxt.text = data.rank;
        rankIcon.sprite = rankSprites[SetRankSprite(data.rank)];
        pxmNameTxt.text = data.name;
        pxmIcon.sprite = data.icon;
        lvTxt.text = string.Format($"{myData.lv}/50");
        SetStars();
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
}
