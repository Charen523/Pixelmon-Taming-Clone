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
                UpdateRewardTxt();
                break;
            default:
                Debug.LogWarning("업데이트 인덱스 오류");
                break;
        }
    }

    public void UpdateRewardTxt()
    {
        string newGoldTxt = saveManager.userData.gold.ToString();
        if (goldTxt.text != newGoldTxt)
        {
            if (goldCoroutine != null)
            {
                StopCoroutine(goldCoroutine);
            }
            goldCoroutine = StartCoroutine(AnimateTextChange(goldTxt, int.Parse(goldTxt.text), int.Parse(newGoldTxt)));
        }

        string newGemTxt = saveManager.userData.diamond.ToString();
        if (gemTxt.text != newGemTxt)
        {
            if (gemCoroutine != null)
            {
                StopCoroutine(gemCoroutine);
            }
            gemCoroutine = StartCoroutine(AnimateTextChange(gemTxt, int.Parse(gemTxt.text), int.Parse(newGemTxt)));
        }
    }

    public void UpdateExpUI()
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

    private IEnumerator AnimateTextChange(TextMeshProUGUI textElement, int startValue, int endValue)
    {
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            int currentValue = (int)Mathf.Lerp(startValue, endValue, elapsed / duration);
            textElement.text = currentValue.ToString();
            yield return null;
        }

        textElement.text = endValue.ToString();
    }
}
