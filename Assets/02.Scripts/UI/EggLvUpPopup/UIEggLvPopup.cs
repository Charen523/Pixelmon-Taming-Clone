using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField] private GameObject Clock;
    [SerializeField] private GameObject Skip;
    [SerializeField] private TextMeshProUGUI TimeTxt;
    private float totalTime => userData.eggLv * 1800f; // 레벨*30분
    private float remainingTime;
    #endregion

    private string[] descs = { "Lv업 게이지", "Lv업 중" };       
    private SaveManager SaveManager;
    private UserData userData;

    private void Start()
    {
        Debug.Log("Start userData.isLvUpMode : " + userData.isLvUpMode);
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

    private void OnEnable()
    {
        SaveManager = SaveManager.Instance;
        userData = SaveManager.userData;
        Debug.Log("OnEnable userData.isLvUpMode : " + userData.isLvUpMode);
        if (userData.isLvUpMode) // Lv업 중
        {
            StartCoroutine(UpdateTimer());
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
        Debug.Log("after fullGaugeCnt : " + userData.fullGaugeCnt);

        if (userData.fullGaugeCnt == lvUpGauges.Count)
        {
            LvUpBtn.interactable = true;
            GaugeUpBtn.interactable = false;
        }
    }

    private void UpdateLvAndRateUI()
    {
        CurLvNum.text = userData.eggLv.ToString();
        NextLvNum.text = (userData.eggLv + 1).ToString();
    }

    private void SetGaugeMode()
    {
        SaveManager.SetData(nameof(userData.isLvUpMode), false);
        Desc.text = descs[0];

        Gauges.gameObject.SetActive(true);
        GaugeAndLvUp.SetActive(true);
        Clock.SetActive(false);
        Skip.SetActive(false);

        for (int i = 0; i < userData.eggLv / 5 + 2; i++)
            lvUpGauges.Add(Instantiate(LvUpGauge, Gauges));

        price = CalculateLevelUpCost(userData.eggLv);
        PriceTxt.text = price.ToString();
        LvUpBtn.interactable = false;
    }

    public void SetLvUpMode()
    {
        Desc.text = descs[1];
        SaveManager.SetData(nameof(userData.isLvUpMode), true);
        SaveManager.SetData(nameof(userData.fullGaugeCnt), 0);
        foreach (var gauge in lvUpGauges)
            gauge.ResetGauge();

        Gauges.gameObject.SetActive(false);
        GaugeAndLvUp.SetActive(false);
        Clock.SetActive(true);
        Skip.SetActive(true);

        // 앱이 다시 시작될 때 경과된 시간 계산
        DateTime lastQuitTime;
        if (DateTime.TryParse(userData.lastLvUpTime, out lastQuitTime))
        {
            TimeSpan elapsedTime = DateTime.Now - lastQuitTime;
            remainingTime = totalTime - (float)elapsedTime.TotalSeconds;
            if (remainingTime <= 0) // 시간이 이미 지났다면 바로 레벨 업
            {   
                remainingTime = 0;
                LvUp();
                return;
            }
        }
        else
        {
            remainingTime = totalTime;
        }
        StartCoroutine(UpdateTimer());
    }

    private void LvUp()
    {
        SaveManager.SetData(nameof(userData.lastLvUpTime), null);
        SaveManager.SetDeltaData(nameof(userData.eggLv), 1);
        if (userData.eggLv % 5 == 0)
            lvUpGauges.Add(Instantiate(LvUpGauge, Gauges));
        UpdateLvAndRateUI();       
        SetGaugeMode();
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= 1f; // 1초마다 감소
            UpdateTimerText();
            yield return new WaitForSeconds(1f); // 1초 대기
        }
        remainingTime = 0;
        UpdateTimerText();
        LvUp();
    }

    private void UpdateTimerText()
    {
        int hours = Mathf.FloorToInt(remainingTime / 3600f);
        int minutes = Mathf.FloorToInt((remainingTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        TimeTxt.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    private void OnDisable()
    {
        if (userData.isLvUpMode)
        {
            SaveManager.SetData(nameof(userData.lastLvUpTime), DateTime.Now.ToString());
            StopCoroutine(UpdateTimer());
        }
    }

    private void OnApplicationQuit()
    {
        if (userData.isLvUpMode)
        {
            SaveManager.SetData(nameof(userData.lastLvUpTime), DateTime.Now.ToString());
            StopCoroutine(UpdateTimer());
        }
    }
}
