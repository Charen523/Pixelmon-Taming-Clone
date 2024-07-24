using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonPopUP : UIBase
{
    [SerializeField]
    private PixelmonTab pxmTab;
    [Header("UI")]
    [SerializeField]
    private Image infoIcon;
    [SerializeField]
    private TextMeshProUGUI infoBoxTxt;
    [SerializeField]
    private TextMeshProUGUI feedCount;
    [SerializeField]
    private TextMeshProUGUI ComposeCount;

    public int infoIndex = 0;
    [SerializeField]
    private PixelmonData data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowPopUp(int dataIndex, PixelmonTab tab)
    {
        pxmTab = tab; 
        infoIndex = dataIndex;
        SetData();
    }

    public void OnPreviousInfo()
    {
        infoIndex--;
        SetData();
    }
    public void OnNextInfo()
    {
        infoIndex++;
        SetData();
    }

    private void SetData()
    {
        data = pxmTab.allData[infoIndex].pxmData;
        InitInfo();
    }


    private void InitInfo()
    {

    }
}
