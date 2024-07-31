using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEggLvPopup : UIBase
{
    #region [SerializeField] private 필드
    [SerializeField] private TextMeshProUGUI Desc;

    [SerializeField] private Button GaugeUpBtn;
    [SerializeField] private TextMeshProUGUI PriceTxt;

    [SerializeField] private Button LvUpBtn;

    [SerializeField] private Transform Gauges;
    [SerializeField] private LvUpGauge LvUpGauge;   
    #endregion

    #region private 필드
    private List<LvUpGauge> lvUpGauges = new List<LvUpGauge>();

    private string[] descs = { "Lv업 게이지", "Lv업 중" };
    private bool isLvUpMode;
    private int price;
    private UserData userData => SaveManager.Instance.userData;

    private int baseGoldCost = 100; // 기본 골드 소모량
    private int increaseRate = 50; // 골드 증가율
    #endregion

    private void Start()
    {
        LvUpGauge gauge1 = Instantiate(LvUpGauge, Gauges);
        LvUpGauge gauge2 = Instantiate(LvUpGauge, Gauges);

        lvUpGauges.Add(gauge1);
        lvUpGauges.Add(gauge2);
    }

    public void SetPopup()
    {
        if (!isLvUpMode) // Lv업 게이지
        {
            Desc.text = descs[0];
            price = CalculateLevelUpCost(userData.eggLv);
            PriceTxt.text = price.ToString();
            LvUpBtn.interactable = false;
        }
        else // Lv업 중
        {
            Desc.text = descs[1];
        }      
    }

    public int CalculateLevelUpCost(int level)
    {
        // 계차 수열 방식으로 골드 소모량 계산
        int goldCost = baseGoldCost + (level * (level + 1) / 2) * increaseRate;
        return goldCost;
    }

    public void OnClickGaugeUpBtn()
    {
        if (userData.gold >= price)
            lvUpGauges[userData.fullGaugeCnt++].GaugeUp();

        if(userData.fullGaugeCnt == lvUpGauges.Count)
        {
            LvUpBtn.interactable = true;
            GaugeUpBtn.interactable = false;
        }
    }

    public void OnClickLvUpBtn()
    {
        SaveManager.Instance.SetDeltaData(nameof(userData.eggLv), 1);
    }
}
