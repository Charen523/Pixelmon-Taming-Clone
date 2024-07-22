using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITopBar : UIBase
{
    private SaveManager invenManager;

    #region User Info UI
    [SerializeField] private Image characImg;
    [SerializeField] private TextMeshProUGUI lvTxt;

    [SerializeField] private TextMeshProUGUI userName;
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private TextMeshProUGUI gemTxt;

    [SerializeField] private Slider expSldr;
    [SerializeField] private TextMeshProUGUI expTxt;
    #endregion

    #region Data Fields
    private int currentExp; 
    private readonly float tempMaxExp = 1000; //임시 변수
    #endregion

    private void Awake()
    {
        invenManager = SaveManager.Instance;
    }

    private void Start()
    {
        InitTopUIData();
    }

    private void InitTopUIData()
    {
        lvTxt.text = $"Lv.{invenManager.userData.userLv}";
        
        userName.text = invenManager.userData.userName;
        goldTxt.text = invenManager.userData.gold.ToString();
        gemTxt.text = invenManager.userData.diamond.ToString();

        currentExp = invenManager.userData.userExp;
        expSldr.value = currentExp / tempMaxExp;
        expTxt.text = (expSldr.value * 100).ToString("0.00") + "%";
    }
}
