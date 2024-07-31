using TMPro;
using UnityEngine;

public enum UpgradeIndex
{
    Atk,
    Cri,
    CriDmg,
    Dmg,
    SDmg,
    SCri,
    SCriDmg
}

public class UpgradeSlot : MonoBehaviour
{
    public UpgradeTab upgradeTab;
    public UpgradeIndex slotIndex;

    #region UI
    [SerializeField] protected TextMeshProUGUI slotLevelTxt; //Lv.(숫자)
    [SerializeField] protected TextMeshProUGUI slotValueTxt; //경우에 따라 % 붙음.
    [SerializeField] private TextMeshProUGUI slotGoldTxt; //끝에 G 붙음.

    [SerializeField] private GameObject GoldBtn; //만렙일 떄 비활성화
    #endregion

    [SerializeField] protected float multiplier = 1.616f;
    [SerializeField] private float commonDiff;

    private int _curLv;
    public int CurLv
    {
        get
        {
            return _curLv;
        }
        set
        {
            if (value >= 1000)
            {
                _curLv = 1000;
                GoldBtn.SetActive(false);
            }

            if (_curLv != value)
            {
                _curLv = value;
                //UpgradeTab쪽의 배열 업데이트 실행
                curLvPrice = MultiplyPrice(_curLv + 1);
            }

            SetTxt();
        }
    }
    private int curBtnLv;

    /*upgrade Stat 수치.*/
    private float _curValue;
    public float CurValue
    {
        get
        {
            return _curValue;
        }
        set
        {
            _curValue = value;
            SetUpgradeStat(value);
        }
    }
    protected float nextValue;

    private int curGold => SaveManager.Instance.userData.gold;
    private int curLvPrice;
    private int curBtnPrice;

    private void Start()
    {
        InitSlot();
        SetTxt();
    }

    private void InitSlot()
    {
        curBtnLv = CurLv + 1;

        CurValue = CalculateValue(CurLv);
        nextValue = CalculateValue(curBtnLv);


    }

    #region UI Methods
    public void BuyBtn()
    {
        if (curBtnPrice <= curGold)
        {
            SaveManager.Instance.SetDeltaData(nameof(SaveManager.Instance.userData.gold), -curBtnPrice);
            CurLv = curBtnLv;
            //레벨 오르고 새로운 가격 PixelmonTab으로 보내고 BtnPrice 새로 계산.
        }
    }
    
    protected virtual void SetTxt()
    {
        slotLevelTxt.text = "Lv." + CurLv.ToString();
        slotValueTxt.text = NumberFormatter.FormatIntNum(CurValue) + "%";
        SetGold();
    }

    protected void SetGold()
    {
        slotGoldTxt.text = NumberFormatter.FormatIntNum(curBtnPrice) + " G";
    }
    #endregion

    #region Value Methods
    private void SetUpgradeStat(float value)
    {
        var fieldname = slotIndex.ToString();
        var upgradeStatType = PixelmonManager.Instance.upgradeStatus.GetType();
        var fieldInfo = upgradeStatType.GetField(fieldname);
        fieldInfo.SetValue(PixelmonManager.Instance.upgradeStatus, value);
        
        //테스트용
        //Debug.Log(fieldInfo.GetValue(PixelmonManager.Instance.upgradeStatus));
    }

    protected virtual float CalculateValue(int reachLv)
    {
        if (slotIndex == UpgradeIndex.Atk)
        {
            return SequenceTool.SumRateSeries(CurValue, CurLv, reachLv, multiplier);
        }
        else
        {
            return SequenceTool.SumDiffSeries(CurValue, CurLv, reachLv, commonDiff);
        }
    }
    #endregion

    #region Price Methods
    public void CalculatePrice(int mulValue)
    {
        if (mulValue == 0)
        {
            curBtnPrice = MaxPrice();
        }
        else
        {
            curBtnLv = CurLv + mulValue;
            curBtnPrice = MultiplyPrice(curBtnLv);
        }

        SetGold();
    }
    
    private int MultiplyPrice(int reachLv)
    {
        float totalCost = curLvPrice * (Mathf.Pow(multiplier, reachLv - CurLv) - 1) / (multiplier - 1);
        return (int)totalCost;
    }
    
    private int MaxPrice()
    {
        int btnPrice = curLvPrice;
        curBtnLv = CurLv + 1;

        while (btnPrice < curGold)
        {
            btnPrice = (int)(btnPrice * multiplier);
            curBtnLv++;
        }

        return btnPrice;
    }
    #endregion
}
