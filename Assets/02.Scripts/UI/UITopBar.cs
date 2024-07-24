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

        expSldr.value = currentExp / tempMaxExp;
        expTxt.text = (expSldr.value * 100).ToString("0.00") + "%";
    }

    private void UpdateTopUI(int index)
    {
        switch(index)
        {
            case 0:
                UpdateRewardTxt();
                break;
            case 1:
                UpdateExpTxt();
                break;
            default:
                Debug.LogWarning("업데이트 인덱스 오류");
                break;
        }
    }

    public void UpdateRewardTxt()
    {
        goldTxt.text = saveManager.userData.gold.ToString();
        gemTxt.text = saveManager.userData.diamond.ToString();
    }

    public void UpdateExpTxt()
    {
        while (currentExp > tempMaxExp)
        {
            saveManager.SetDeltaData("userExp", -tempMaxExp);
            saveManager.userData.userLv++;
            lvTxt.text = $"Lv.{saveManager.userData.userLv}";
        }

        expSldr.value = currentExp / tempMaxExp;
        expTxt.text = (expSldr.value * 100).ToString("0.00") + "%";
    }
}
