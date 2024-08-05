using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

    private BigInteger price;
    private List<LvUpGauge> lvUpGauges = new List<LvUpGauge>();
    #endregion

    #region Lv업 중
    [SerializeField] private GameObject Clock;
    [SerializeField] private GameObject Skip;
    [SerializeField] private Button DiaBtn;
    [SerializeField] private TextMeshProUGUI TimeTxt;
    private float totalTime => userData.eggLv * 1800f; // 레벨*30분
    private float remainingTime;

    private int skipDia = 1000;
    #endregion

    private string[] descs = { "Lv업 게이지", "Lv업 중" };
    private UIMiddleBar uiMiddleBar;
    private UserData userData => SaveManager.Instance.userData;
    private Coroutine updateTimerCoroutine;

    private void Start()
    {
        for (int i = 0; i < userData.eggLv / 5 + 2; i++)
        {
            lvUpGauges.Add(Instantiate(LvUpGauge, Gauges));
            if(i < userData.fullGaugeCnt)
                lvUpGauges[i].GaugeUp();
        }        
    }

    public void SetPopup(UIMiddleBar middleBar)
    {
        uiMiddleBar = middleBar;
        UpdateLvAndRateUI();

        if (userData.isLvUpMode) // Lv업 중
            SetLvUpMode();
        else // Lv업 게이지
            SetGaugeMode();
    }

    private void UpdateLvAndRateUI()
    {
        CurLvNum.text = userData.eggLv.ToString();
        NextLvNum.text = (userData.eggLv + 1).ToString();
    }
    private void SetLvUpBtn()
    {
        if (userData.fullGaugeCnt == lvUpGauges.Count)
        {
            LvUpBtn.interactable = true;
            GaugeUpBtn.interactable = false;
        }
        else
        {
            LvUpBtn.interactable = false;
            GaugeUpBtn.interactable = true;
        }
    }

    private void SetGaugeUpBtn()
    {
        if(userData.gold >= price)
            GaugeUpBtn.interactable = true;
        else GaugeUpBtn.interactable = false;
    }

    private void SetDiaBtn()
    {
        if(userData.diamond >= skipDia)
            DiaBtn.interactable = true;
        else DiaBtn.interactable = false;
    }

    private void SetLvUpMode()
    {
        Desc.text = descs[1];
        SaveManager.Instance.SetFieldData(nameof(userData.isLvUpMode), true);

        Gauges.gameObject.SetActive(false);
        GaugeAndLvUp.SetActive(false);
        Clock.SetActive(true);
        Skip.SetActive(true);

        SetDiaBtn();

        updateTimerCoroutine = StartCoroutine(UpdateTimer());
    }

    private void SetGaugeMode()
    {
        SaveManager.Instance.SetFieldData(nameof(userData.isLvUpMode), false);
        Desc.text = descs[0];

        Gauges.gameObject.SetActive(true);
        GaugeAndLvUp.SetActive(true);
        Clock.SetActive(false);
        Skip.SetActive(false);

        price = Calculater.CalPrice(userData.eggLv, 1000, 100, 50);
        PriceTxt.text = Calculater.NumFormatter(price);
        SetLvUpBtn();
        SetGaugeUpBtn();
    }

    public void OnClickGaugeUpBtn()
    {
        lvUpGauges[userData.fullGaugeCnt].GaugeUp();
        SaveManager.Instance.SetFieldData(nameof(userData.fullGaugeCnt), 1, true);
        SaveManager.Instance.SetFieldData(nameof(userData.gold), -price, true);
        SetLvUpBtn();
        SetGaugeUpBtn();
    }

    public void OnClickLvUpBtn()
    {
        SaveManager.Instance.SetFieldData(nameof(userData.startLvUpTime), DateTime.Now.ToString());
        SaveManager.Instance.SetFieldData(nameof(userData.fullGaugeCnt), 0);
        foreach (var gauge in lvUpGauges)
            gauge.ResetGauge();
        if ((userData.eggLv + 1) % 5 == 0)
            lvUpGauges.Add(Instantiate(LvUpGauge, Gauges));
        SetLvUpMode();
    }

    //TODO : 광고넣기
    public void OnClickAdBtn()
    {

    }

    public void OnClickDiaBtn()
    {
        SaveManager.Instance.SetFieldData(nameof(userData.diamond), -skipDia, true);
        SaveManager.Instance.SetFieldData(nameof(userData.skipTime), userData.skipTime + 3600f);
        SetDiaBtn();
        remainingTime -= 3600f;
        if(remainingTime <= 0) SetGaugeMode();
    }

    private void LvUp()
    {
        SaveManager.Instance.SetFieldData(nameof(userData.startLvUpTime), null);
        SaveManager.Instance.SetFieldData(nameof(userData.eggLv), 1, true);
        uiMiddleBar.SetEggTextUI();
        UpdateLvAndRateUI();       
        SetGaugeMode();
    }

    private IEnumerator UpdateTimer()
    {
        // 앱이 다시 시작될 때 경과된 시간 계산
        TimeSpan elapsedTime = DateTime.Now - DateTime.Parse(userData.startLvUpTime);
        remainingTime = totalTime - (float)elapsedTime.TotalSeconds - userData.skipTime;

        while (remainingTime > 0)
        {
            remainingTime -= 1f; // 1초마다 감소
            UpdateTimerText();
            yield return new WaitForSeconds(1f); // 1초 대기
        }
        SaveManager.Instance.SetFieldData(nameof(userData.skipTime), 0);
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
        if (updateTimerCoroutine != null)
        {
            StopCoroutine(updateTimerCoroutine);
            updateTimerCoroutine = null;
        }
    }
}
