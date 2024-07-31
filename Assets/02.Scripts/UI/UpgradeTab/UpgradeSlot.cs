using System.Numerics;
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

    [SerializeField] protected float multiplier = 1.1f;
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
                upgradeTab.SetUpgradeLvs((int)slotIndex, value);
            }

            SetTxt();
        }
    }
    private int curBtnLv;

    /*upgrade Stat 수치.*/
    [SerializeField] private float _curValue => CurLv;
    public float CurValue
    {
        get
        {
            return _curValue;
        }
        set
        {
            //SetUpgradeStat(value);
        }
    }
    protected float nextValue;

    private BigInteger curGold => SaveManager.Instance.userData.gold;
    private BigInteger curLvPrice => CalculateTool.GetPrice(CurLv);
    private BigInteger curBtnPrice;

    private int curMulValue = 1;

    private void Start()
    {
        InitSlot();
        SetTxt();
    }

    private void InitSlot()
    {
        curBtnLv = CurLv + 1;

        if (CurLv == 1)
        {
            if (slotIndex == UpgradeIndex.Atk)
            {
                CurValue = 1;
            }
            else
            {
                CurValue = 0;
            }
        }
        else
        {
            CurValue = CalculateValue(CurLv);
        }
        nextValue = CalculateValue(curBtnLv);

        CalculatePrice(curMulValue);
    }

    #region UI Methods
    public void BuyBtn()
    {
        if (curBtnPrice <= curGold)
        {
            SaveManager.Instance.SetGold(-curBtnPrice, true);
            CurLv = curBtnLv;
            CalculatePrice(curMulValue);
            SetTxt();
            //레벨 오르고 새로운 가격 PixelmonTab으로 보내고 BtnPrice 새로 계산.
        }
        else
        {
            Debug.LogWarning("돈 부족함!");
        }
    }

    protected virtual void SetTxt()
    {
        slotLevelTxt.text = "Lv. " + CurLv.ToString();
        slotValueTxt.text = CurValue + "%";
        SetGoldTxt();
    }

    protected void SetGoldTxt()
    {
        slotGoldTxt.text = CalculateTool.NumFormatter(curBtnPrice);
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
            return CalculateTool.SumRateSeries(CurValue, CurLv, reachLv, multiplier);
        }
        else
        {
            return CalculateTool.SumDiffSeries(CurValue, CurLv, reachLv, commonDiff);
        }
    }
    #endregion

    #region Price Methods
    public void CalculatePrice(int mulValue)
    {
        curMulValue = mulValue;

        if (mulValue == 0)
        {
            FindMaxPrice();
        }
        else
        {
            curBtnLv = CurLv + mulValue;
            curBtnPrice = CalculateTool.GetTotalPrice(CurLv, curBtnLv);
        }

        SetGoldTxt();
    }

    private void FindMaxPrice()
    {
        curBtnLv = CurLv;
        do
        {
            curBtnLv++;
            curBtnPrice = CalculateTool.GetTotalPrice(CurLv, curBtnLv);
        } 
        while (CalculateTool.GetTotalPrice(CurLv, curBtnLv + 1) <= curGold);
    }
    #endregion
}
