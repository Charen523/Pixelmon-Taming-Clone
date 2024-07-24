using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITopBar : UIBase
{
    private SaveManager saveManager;

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
    private int currentExp => saveManager.userData.userExp; 
    private readonly int tempMaxExp = 1000; //임시 변수
    #endregion

    #region Coroutine
    private Coroutine goldCoroutine;
    private Coroutine gemCoroutine;
    private Coroutine expCoroutine;
    #endregion

    private void Awake()
    {
        saveManager = SaveManager.Instance;
    }

    private void Start()
    {
        InitTopUIData();
        saveManager.UpdateUI += UpdateTopUI;
    }

    private void InitTopUIData()
    {
        lvTxt.text = $"Lv.{saveManager.userData.userLv}";
        
        userName.text = saveManager.userData.userName;
        goldTxt.text = saveManager.userData.gold.ToString();
        gemTxt.text = saveManager.userData.diamond.ToString();

        UpdateExpUI();
    }

    private void UpdateTopUI(int index)
    {
        switch(index)
        {
            case 0:
                UpdateExpUI();
                break;
            case 1:
                UpdateRewardUI();
                break;
            default:
                Debug.LogWarning("업데이트 인덱스 오류");
                break;
        }
    }

    public void UpdateRewardUI()
    {
        string newGoldTxt = saveManager.userData.gold.ToString();
        if (goldTxt.text != newGoldTxt)
        {
            if (goldCoroutine != null)
            {
                StopCoroutine(goldCoroutine);
            }
            goldCoroutine = StartCoroutine(UIUtils.AnimateTextChange(goldTxt, int.Parse(goldTxt.text), int.Parse(newGoldTxt)));
        }

        string newGemTxt = saveManager.userData.diamond.ToString();
        if (gemTxt.text != newGemTxt)
        {
            if (gemCoroutine != null)
            {
                StopCoroutine(gemCoroutine);
            }
            gemCoroutine = StartCoroutine(UIUtils.AnimateTextChange(gemTxt, int.Parse(gemTxt.text), int.Parse(newGemTxt)));
        }
    }

    public void UpdateExpUI()
    {
        int startExp = currentExp;

        while (currentExp > tempMaxExp)
        {
            saveManager.SetDeltaData("userExp", -tempMaxExp);
            saveManager.userData.userLv++;
            lvTxt.text = $"Lv.{saveManager.userData.userLv}";
        }

        int endExp = currentExp;

        if (expCoroutine != null)
        {
            StopCoroutine(expCoroutine);
        }

        float tempExp = currentExp / (float)tempMaxExp;

        expCoroutine = StartCoroutine(UIUtils.AnimateSliderChange(expSldr, startExp, endExp, tempMaxExp, 1));
        expTxt.text = (tempExp * 100).ToString("0.00") + "%";
    }
}