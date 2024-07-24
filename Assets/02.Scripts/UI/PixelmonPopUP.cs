using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonPopUP : UIBase
{
    [SerializeField]
    private PixelmonTab tab;
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

    public void ShowPopUp(int dataIndex)
    {
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
        data = tab.allData[infoIndex].pxmData;
        InitInfo();
    }


    private void InitInfo()
    {

    }
}
