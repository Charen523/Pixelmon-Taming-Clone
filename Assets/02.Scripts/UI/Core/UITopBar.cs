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
        UIManager.Instance.UpdateUI += UpdateTopUI;
    }

    private void InitTopUIData()
    {
        lvTxt.text = $"Lv.{saveManager.userData.userLv}";
        
        userName.text = saveManager.userData.userName;
        UpdateGoldUI();
        gemTxt.text = saveManager.userData.diamond.ToString();

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
        goldTxt.text = CalculateTool.NumFormatter(saveManager.userData.gold);
    }

    public void UpdateDiamondUI()
    {
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