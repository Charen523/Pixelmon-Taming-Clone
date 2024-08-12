using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITopBar : UIBase
{
    private SaveManager saveManager;
    private UserData userData;

    #region User Info UI
    [SerializeField] private Image characImg;
    [SerializeField] private TextMeshProUGUI lvTxt;
    [SerializeField] private TextMeshProUGUI nameTxt;

    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private TextMeshProUGUI gemTxt;

    [SerializeField] private Slider expSldr;
    [SerializeField] private TextMeshProUGUI expTxt;
    #endregion

    #region Data Fields
    [SerializeField] private int bNum = 1000;
    [SerializeField] private int d1 = 1000;
    [SerializeField] private int d2 = 500;

    private BigInteger currentExp => saveManager.userData.userExp; 
    private BigInteger tempMaxExp => Calculater.CalPrice(userData.userLv, bNum, d1, d2); //임시 변수

    private float prevExp = 0;
    private float curExp = 0;
    #endregion

    #region Coroutine
    private Coroutine goldCoroutine;
    private Coroutine gemCoroutine;
    private Coroutine expCoroutine;
    #endregion

    protected override void Awake()
    {
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
    }

    private void Start()
    {
        InitTopUIData();
        UIManager.Instance.UpdateUI += UpdateTopUI;
    }

    private void InitTopUIData()
    {
        lvTxt.text = $"Lv.{saveManager.userData.userLv}";
        nameTxt.text = saveManager.userData.userName;
        UpdateGoldUI();
        UpdateDiamondUI();
        UpdateExpUI();
    }

    private void UpdateTopUI(DirtyUI dirtyUI)
    {
        switch(dirtyUI)
        {
            case DirtyUI.Gold:
                UpdateGoldUI();
                break;
            case DirtyUI.Diamond:
                UpdateDiamondUI();
                break;
            case DirtyUI.UserExp:
                UpdateExpUI();
                break;
            default:
                break;
        }
    }

    public void UpdateGoldUI()
    {
        goldTxt.text = Calculater.NumFormatter(saveManager.userData.gold);
    }

    public void UpdateDiamondUI()
    {
        gemTxt.text = Calculater.NumFormatter(saveManager.userData.diamond);
    }

    public void UpdateExpUI()
    {
        BigInteger prevScaleExp = currentExp * 10000 / tempMaxExp;
        prevExp = (float)prevScaleExp / 10000;

        while (currentExp > tempMaxExp)
        {
            saveManager.SetFieldData(nameof(userData.userExp), -tempMaxExp, true);
            userData.userLv++;
            lvTxt.text = $"Lv.{userData.userLv}";
            PixelmonManager.Instance.unlockSlotAction?.Invoke(userData.userLv);
            if (QuestManager.Instance.isUserLevelQ)
            {
                QuestManager.Instance.OnQuestEvent();
            }
        }

        BigInteger endExp = currentExp;

        if (expCoroutine != null)
        {
            StopCoroutine(expCoroutine);
        }

        BigInteger tempScaleExp = currentExp * 10000 / tempMaxExp;
        curExp = (float)tempScaleExp / 10000;

        expCoroutine = StartCoroutine(UIUtils.AnimateSliderChange(expSldr, prevExp, curExp, 1));
        expTxt.text = (curExp * 100).ToString("0.00") + "%";
    }
}