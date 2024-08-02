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

public abstract class UpgradeSlot : MonoBehaviour
{
    protected BigInteger ownGold => SaveManager.Instance.userData.gold;

    [Header("Slot Info")]
    [HideInInspector] public UpgradeTab upgradeTab;
    [SerializeField] private UpgradeIndex slotIndex; //Inspector Setting
    [SerializeField] protected int maxLv; //Inspector Setting

    [Header("Upgrade UI")]
    [SerializeField] private TextMeshProUGUI slotLevelTxt;
    [SerializeField] protected TextMeshProUGUI slotValueTxt;
    [SerializeField] private TextMeshProUGUI slotPriceTxt;
    [SerializeField] private GameObject GoldBtn; //TODO: 끝까지 불필요하면 삭제.

    private int _curLv;
    public int CurLv
    {
        get
        {
            return _curLv;
        }
        set
        {
            if (_curLv != value)
            {
                _curLv = value;

                if (_curLv > maxLv)
                {
                    _curLv = maxLv;
                    Debug.LogWarning("CurLv에 부여된 값이 maxLv보다 큽니다.");
                }

                if (_curLv >= maxLv)
                {
                    slotPriceTxt.text = "<color=FFFF00>MAX</color>";
                }

                if (isStart)
                {
                    upgradeTab.SaveUpgradeLvs((int)slotIndex, _curLv);
                }
            }
        }
    }

    /*upgrade Stat 현재 값: PixelmonManager의 Status와 동일해야 함!*/
    private float _curValue;
    protected float CurValue
    {
        get
        {
            return _curValue;
        }
        set
        {
            if (_curValue != value)
            {
                _curValue = value;
                GiveChangedStat(_curValue);
                PixelmonStatHandler.ApplyMyPixelmon(slotIndex);
            }
        }
    }

    protected BigInteger curLvPrice => CalculateTool.GetAtkPrice(CurLv);

    #region Buy Button Values
    protected int nextLv;
    protected float nextValue;
    protected BigInteger nextPrice;
    #endregion

    protected int curUpgradeRate = 1;
    private bool isStart = false;

    private void Start()
    {
        InitSlot();
        SetSlotTxts();
        isStart = true;
    }

    private void InitSlot()
    {
        nextLv = CurLv + 1;

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
            CurValue = ValuePerLv(CurLv);
        }
        nextValue = ValuePerLv(nextLv);

        CalculatePrice(curUpgradeRate);
    }

    #region UI Methods
    public void BuyBtn()
    {
        if (nextPrice <= ownGold)
        {
            CurLv = nextLv;
            CurValue = nextValue;

            CalculatePrice(curUpgradeRate);
            SaveManager.Instance.SetGold(-nextPrice, true);

            SetSlotTxts();
        }
        else
        {
            Debug.LogWarning("돈 부족함!");
        }
    }

    protected virtual void SetSlotTxts()
    {
        slotLevelTxt.text = "Lv." + CurLv.ToString();
        SetGoldTxt();
    }

    //TODO: 잘하면 없어질듯?
    protected void SetGoldTxt()
    {
        slotPriceTxt.text = CalculateTool.NumFormatter(nextPrice);
    } 
    #endregion

    #region Value Methods
    private void GiveChangedStat(float value)
    {
        var fieldname = slotIndex.ToString();
        var upgradeStatType = PixelmonManager.Instance.upgradeStatus.GetType();
        var fieldInfo = upgradeStatType.GetField(fieldname);
        fieldInfo.SetValue(PixelmonManager.Instance.upgradeStatus, value);
    }

    protected abstract float ValuePerLv(int reachLv);
    #endregion

    #region Price Methods
    public abstract void CalculatePrice(int mulValue);
    protected abstract void FindMaxPrice();
    #endregion
}
