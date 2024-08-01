using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEggLvPopup : UIBase
{
    [SerializeField] private TextMeshProUGUI CurLvNum;
    [SerializeField] private TextMeshProUGUI NextLvNum;

    #region Lv업 게이지
    [SerializeField] private TextMeshProUGUI Desc;
    [SerializeField] private TextMeshProUGUI PriceTxt;
    [SerializeField] private Button GaugeUpBtn;
    [SerializeField] private Button LvUpBtn;
    [SerializeField] private Transform Gauges;
    [SerializeField] private LvUpGauge LvUpGauge;
    [SerializeField] private GameObject GaugeAndLvUp;

    private int baseGoldCost = 100; // 기본 골드 소모량
    private int increaseRate = 50; // 골드 증가율
    private int price;
    private List<LvUpGauge> lvUpGauges = new List<LvUpGauge>();
    #endregion

    #region Lv업 중
    [SerializeField] private GameObject Time;
    [SerializeField] private GameObject Skip;
    [SerializeField] private TextMeshProUGUI TimeTxt;
    private float totalTime => userData.eggLv * 1800f; // 레벨*30분
    #endregion

    private string[] descs = { "Lv업 게이지", "Lv업 중" };       
    private UserData userData => SaveManager.Instance.userData;

    private void Start()
    {
        Debug.Log("start fullGaugeCnt : " + userData.fullGaugeCnt);
        UpdateLvAndRateUI();

        if (userData.isLvUpMode) // Lv업 중
        {
            SetLvUpMode();
        }
        else // Lv업 게이지
        {
            SetGaugeMode();
        }
    }

    public void SetPopup()
    {
  
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
        Debug.Log("after fullGaugeCnt : " + userData.fullGaugeCnt);

        if (userData.fullGaugeCnt == lvUpGauges.Count)
        {
            LvUpBtn.interactable = true;
            GaugeUpBtn.interactable = false;
        }
    }

    public void OnClickLvUpBtn()
    {
        SaveManager.Instance.SetDeltaData(nameof(userData.eggLv), 1);
        SaveManager.Instance.SetData(nameof(userData.fullGaugeCnt), 0);

        UpdateLvAndRateUI();
        SetLvUpMode();

        if (userData.eggLv / 5 == 0)
            lvUpGauges.Add(Instantiate(LvUpGauge, Gauges));
    }

    private void UpdateLvAndRateUI()
    {
        CurLvNum.text = userData.eggLv.ToString();
        NextLvNum.text = (userData.eggLv + 1).ToString();
    }

    private void SetGaugeMode()
    {
        SaveManager.Instance.SetData(nameof(userData.isLvUpMode), false);
        Desc.text = descs[0];

        Gauges.gameObject.SetActive(true);
        GaugeAndLvUp.SetActive(true);
        Time.SetActive(false);
        Skip.SetActive(false);

        for (int i = 0; i < userData.eggLv / 5 + 2; i++)
            lvUpGauges.Add(Instantiate(LvUpGauge, Gauges));

        price = CalculateLevelUpCost(userData.eggLv);
        PriceTxt.text = price.ToString();
        LvUpBtn.interactable = false;
    }

    private void SetLvUpMode()
    {
        SaveManager.Instance.SetData(nameof(userData.isLvUpMode), true);
        Desc.text = descs[1];

        foreach (var gauge in lvUpGauges)
            gauge.ResetGauge();

        Gauges.gameObject.SetActive(false);
        GaugeAndLvUp.SetActive(false);
        Time.SetActive(true);
        Skip.SetActive(true);
    }
}
