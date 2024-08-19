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

    #region 확률표
    [SerializeField] private TextMeshProUGUI CommonCurNum;
    [SerializeField] private TextMeshProUGUI CommonNextNum;
    [SerializeField] private TextMeshProUGUI AdvancedCurNum;
    [SerializeField] private TextMeshProUGUI AdvancedNextNum;
    [SerializeField] private TextMeshProUGUI RareCurNum;
    [SerializeField] private TextMeshProUGUI RareNextNum;
    [SerializeField] private TextMeshProUGUI EpicCurNum;
    [SerializeField] private TextMeshProUGUI EpicNextNum;
    [SerializeField] private TextMeshProUGUI LegendaryCurNum;
    [SerializeField] private TextMeshProUGUI LegendaryNextNum;
    #endregion

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

    [SerializeField] private TextMeshProUGUI adsSkipCountTxt;
    private int skipDia = 1000;
    #endregion

    #region 버튼 색상 Sprite
    [SerializeField] private Sprite GaugeUpSprite;
    [SerializeField] private Sprite LvUpSprite;
    [SerializeField] private Sprite SkipSprite;
    [SerializeField] private Sprite GraySprite;
    #endregion

    private string[] descs = { "레벨업 게이지", "레벨업 중", "최대 레벨 도달" };
    private UIMiddleBar uiMiddleBar;
    private UserData userData => SaveManager.Instance.userData;
    private Coroutine updateTimerCoroutine;

    public override void Opened(object[] param) 
    { 
        base.Opened(param);      
        UIManager.Instance.UpdateUI += UpdateEggLvPopupUI;
        
        SetPopup(param[0] as UIMiddleBar);

        UpdateLvAndRateUI();

        for (int i = 0; i < userData.eggLv / 5 + 2; i++)
        {
            lvUpGauges.Add(Instantiate(LvUpGauge, Gauges));
            if (i < userData.fullGaugeCnt)
                lvUpGauges[i].GaugeUp();
        }
    }

    public void SetPopup(UIMiddleBar middleBar)
    {
        uiMiddleBar = middleBar;
        
        if (userData.isLvUpMode) // Lv업 중
            SetLvUpMode();
        else // Lv업 게이지
            SetGaugeMode();
    }

    private void UpdateEggLvPopupUI(DirtyUI dirtyU)
    {
        switch (dirtyU)
        {
            case DirtyUI.EggLv:
                UpdateLvAndRateUI();
                break;
            case DirtyUI.Gold:
                if(GaugeUpBtn.interactable == false) 
                    SetGaugeUpBtn();
                break;
            case DirtyUI.Diamond:
                    SetDiaBtn();
                break;
        }
    }

    private void UpdateLvAndRateUI()
    {
        if (userData.eggLv != 10)
        {         
            NextLvNum.text = (userData.eggLv + 1).ToString();          
            var nextData= DataManager.Instance.GetData<EggRateData>((userData.eggLv + 1).ToString());
          
            CommonNextNum.text = nextData.common.ToString("F2") + "%";           
            AdvancedNextNum.text = nextData.advanced.ToString("F2") + "%";           
            RareNextNum.text = nextData.rare.ToString("F2") + "%";         
            EpicNextNum.text = nextData.epic.ToString("F2") + "%";           
            LegendaryNextNum.text = nextData.legendary.ToString("F2") + "%";
        }
        else if(userData.eggLv == 10)
            NextLvNum.text = "X";

        CurLvNum.text = userData.eggLv.ToString();
        var curData = DataManager.Instance.GetData<EggRateData>(userData.eggLv.ToString());

        CommonCurNum.text = curData.common.ToString("F2") + "%";
        AdvancedCurNum.text = curData.advanced.ToString("F2") + "%";
        RareCurNum.text = curData.rare.ToString("F2") + "%";
        EpicCurNum.text = curData.epic.ToString("F2") + "%";
        LegendaryCurNum.text = curData.legendary.ToString("F2") + "%";
    }

    public enum EggLvBtnType
    {
        GaugeUp,
        LvUp,
        DiaBtn,
        AdBtn
    }
    private void SetBtnSprite(EggLvBtnType type, Button btn, bool isInteractable)
    {
        if (!isInteractable)
        {
            btn.image.sprite = GraySprite;
            btn.interactable = false;
        }            
        else
        {
            btn.interactable = true;
            switch (type)
            {
                case EggLvBtnType.GaugeUp: btn.image.sprite = GaugeUpSprite; break;
                case EggLvBtnType.LvUp: btn.image.sprite = LvUpSprite; break;
                case EggLvBtnType.DiaBtn:
                case EggLvBtnType.AdBtn:
                    btn.image.sprite = SkipSprite; break;
            }
        }
    }

    private void SetLvUpBtn()
    {
        if (userData.eggLv == 10)
        {
            SetBtnSprite(EggLvBtnType.LvUp, LvUpBtn, false);
            SetBtnSprite(EggLvBtnType.GaugeUp, GaugeUpBtn, false);
        }
        else if (userData.fullGaugeCnt == lvUpGauges.Count)
        {
            SetBtnSprite(EggLvBtnType.LvUp, LvUpBtn, true);
            SetBtnSprite(EggLvBtnType.GaugeUp, GaugeUpBtn, false);
        }
        else
        {
            SetBtnSprite(EggLvBtnType.LvUp, LvUpBtn, false);
            SetBtnSprite(EggLvBtnType.GaugeUp, GaugeUpBtn, true);
        }
    }

    private void SetGaugeUpBtn()
    {
        if (userData.fullGaugeCnt == lvUpGauges.Count) return;

        if (userData.gold >= price)
            SetBtnSprite(EggLvBtnType.GaugeUp, GaugeUpBtn, true);
        else
            SetBtnSprite(EggLvBtnType.GaugeUp, GaugeUpBtn, false);
    }

    private void SetDiaBtn()
    {
        if(userData.diamond >= skipDia)
            SetBtnSprite(EggLvBtnType.DiaBtn, DiaBtn, true);           
        else 
            SetBtnSprite(EggLvBtnType.DiaBtn, DiaBtn, false);
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

        if (userData.eggLv == 10) return;
        updateTimerCoroutine = StartCoroutine(UpdateTimer());
    }

    private void SetGaugeMode()
    {
        SaveManager.Instance.SetFieldData(nameof(userData.isLvUpMode), false);
        if (userData.eggLv == 10)
        {
            Desc.text = descs[2];
            Gauges.gameObject.SetActive(false);
            GaugeAndLvUp.SetActive(false);
            PriceTxt.text = "-";
        }
        else if (userData.eggLv < 10)
        {
            Desc.text = descs[0];
            Gauges.gameObject.SetActive(true);
            GaugeAndLvUp.SetActive(true);

            price = Calculater.CalPrice(userData.eggLv, 1000, 100, 50);
            PriceTxt.text = Calculater.NumFormatter(price);
            Canvas.ForceUpdateCanvases();
        }
        Clock.SetActive(false);
        Skip.SetActive(false);
        SetLvUpBtn();
        SetGaugeUpBtn();
    }

    public void OnClickGaugeUpBtn()
    {
        if (userData.eggLv == 10) return;
        lvUpGauges[userData.fullGaugeCnt].GaugeUp();
        SaveManager.Instance.SetFieldData(nameof(userData.fullGaugeCnt), 1, true);
        SaveManager.Instance.SetFieldData(nameof(userData.gold), -price, true);
        SetLvUpBtn();
    }

    public void OnClickLvUpBtn()
    {
        if (userData.eggLv == 10) return;
        SaveManager.Instance.SetFieldData(nameof(userData.startLvUpTime), DateTime.Now.ToString());
        SaveManager.Instance.SetFieldData(nameof(userData.fullGaugeCnt), 0);
        foreach (var gauge in lvUpGauges)
            gauge.ResetGauge();
        if ((userData.eggLv + 1) % 5 == 0)
            lvUpGauges.Add(Instantiate(LvUpGauge, Gauges));
        SetLvUpMode();
    }

    //광고넣기
    public void OnClickAdBtn(int decTime)
    {
        SaveManager.Instance.SetFieldData(nameof(userData.adsCount), --userData.adsCount);
        SaveManager.Instance.SetFieldData(nameof(userData.skipTime), userData.skipTime + decTime);
        adsSkipCountTxt.text = string.Format("{0}/4", userData.adsCount);
        remainingTime -= decTime;
        if (remainingTime <= 0) SetGaugeMode();
    }

    public void OnClickDiaBtn()
    {
        SaveManager.Instance.SetFieldData(nameof(userData.diamond), -skipDia, true);
        SaveManager.Instance.SetFieldData(nameof(userData.skipTime), userData.skipTime + 3600f);
        remainingTime -= 3600f;
        if(remainingTime <= 0) SetGaugeMode();
    }

    private void LvUp()
    {
        StopAllCoroutines();
        SaveManager.Instance.SetFieldData(nameof(userData.startLvUpTime), null);
        SaveManager.Instance.SetFieldData(nameof(userData.eggLv), 1, true);    
        SetGaugeMode();

        if (QuestManager.Instance.IsMyTurn(QuestType.Nest))
        {
            QuestManager.Instance.OnQuestEvent();
        }
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
