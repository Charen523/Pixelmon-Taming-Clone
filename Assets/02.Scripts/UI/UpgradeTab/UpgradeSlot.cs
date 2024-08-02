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
    public UpgradeTab upgradeTab;
    public UpgradeIndex slotIndex;
    protected BigInteger ownGold => SaveManager.Instance.userData.gold;

    #region UI
    [SerializeField] protected TextMeshProUGUI slotLevelTxt; //Lv.(숫자)
    [SerializeField] protected TextMeshProUGUI slotValueTxt; //경우에 따라 % 붙음.
    [SerializeField] protected TextMeshProUGUI slotGoldTxt; //끝에 G 붙음.
    [SerializeField] protected GameObject GoldBtn; //만렙일 떄 비활성화
    #endregion

    [SerializeField] protected int maxLv;
    protected int _curLv;
    public int CurLv
    {
        get
        {
            return _curLv;
        }
        set
        {
            if (value >= maxLv)
            {
                _curLv = maxLv;
                GoldBtn.SetActive(false);
            }

            if (_curLv != value)
            {
                _curLv = value;
                upgradeTab.SetUpgradeLvs((int)slotIndex, value);
            }
        }
    }

    /*upgrade Stat 현재 값: PixelmonManager의 Status와 동일해야 함!*/
    [SerializeField] private float _curValue;
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
            PixelmonStatHandler.ApplyMyPixelmon(slotIndex);
        }
    }

    protected BigInteger curLvPrice => CalculateTool.GetAtkPrice(CurLv);

    /*구매버튼을 누를 시 적용되는 값*/
    protected int nextLv;
    protected float nextValue;
    protected BigInteger nextPrice;

    protected int curMulValue = 1;

    private void Start()
    {
        InitSlot();
        SetTxt();
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
            CurValue = CalculateValue(CurLv);
        }
        nextValue = CalculateValue(nextLv);

        CalculatePrice(curMulValue);
    }

    #region UI Methods
    public void BuyBtn()
    {
        if (nextPrice <= ownGold)
        {
            CurLv = nextLv;
            CurValue = nextValue;

            CalculatePrice(curMulValue);
            SaveManager.Instance.SetGold(-nextPrice, true);

            SetTxt();
        }
        else
        {
            Debug.LogWarning("돈 부족함!");
        }
    }

    protected abstract void SetTxt();

    protected void SetGoldTxt()
    {
        slotGoldTxt.text = CalculateTool.NumFormatter(nextPrice);
    }
    #endregion

    #region Value Methods
    private void SetUpgradeStat(float value)
    {
        var fieldname = slotIndex.ToString();
        var upgradeStatType = PixelmonManager.Instance.upgradeStatus.GetType();
        var fieldInfo = upgradeStatType.GetField(fieldname);
        fieldInfo.SetValue(PixelmonManager.Instance.upgradeStatus, value);
    }

    protected abstract float CalculateValue(int reachLv);
    #endregion

    #region Price Methods
    public abstract void CalculatePrice(int mulValue);
    protected abstract void FindMaxPrice();
    #endregion
}
