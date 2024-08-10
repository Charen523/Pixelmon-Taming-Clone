using TMPro;
using UnityEngine;

public class FarmTab : UIBase
{
    private SaveManager saveManager;
    private UserData userData;

    [SerializeField] public FieldSlot[] fieldSlots;

    #region User Data
    private int seedCount => userData.seed;
    private int foodCount => userData.food;
    #endregion

    #region UI
    [SerializeField] private TextMeshProUGUI seedTxt;
    [SerializeField] private TextMeshProUGUI foodTxt;
    #endregion

    int i = 0;

    private bool isAwakeEnabled;
    protected override void Awake()
    {
        isAwakeEnabled = false;
        base.Awake();
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        UIManager.Instance.UpdateUI += UpdateFieldUI;

        for (int i = 0; i < fieldSlots.Length; i++)
        {
            fieldSlots[i].farmTab = this;
            fieldSlots[i].slotIndex = i;
            fieldSlots[i].fieldData = userData.fieldDatas[i];
        }
        seedTxt.text = seedCount.ToString();
        foodTxt.text = foodCount.ToString();
        isAwakeEnabled = true;
    }

    private void OnEnable()
    {
        if (isAwakeEnabled && i != 0)
        {
            seedTxt.text = seedCount.ToString();
            foodTxt.text = foodCount.ToString();

            for (int i = 0; i < fieldSlots.Length; i++)
            {
                if (fieldSlots[i].fieldData.currentFieldState == FieldState.Seeded)
                {
                    fieldSlots[i].CalculateRemainingTime();
                }
                fieldSlots[i].CurrentFieldAction(fieldSlots[i].CurrentFieldState);
                SetAllPriceTxts();
            }
        }
        i++;
    }

    private void UpdateFieldUI(DirtyUI dirtyUI)
    {
        switch (dirtyUI)
        {
            case DirtyUI.Seed:
                seedTxt.text = seedCount.ToString();
                break;
            case DirtyUI.Food:
                foodTxt.text = foodCount.ToString();
                break;
        }
    }

    public void UnlockNextField(int buyIndex)
    {
        if (buyIndex > 4) return;
        fieldSlots[buyIndex + 1].CurrentFieldState = FieldState.Buyable;
        SaveFarmData();
    }

    public bool PlantSeed()
    {
        if (seedCount == 0)
        {
            Debug.LogWarning("씨앗 없음!");
            return false;
        }
        else
        {
            saveManager.SetFieldData(nameof(saveManager.userData.seed), -1, true);
            return true;
        }
    }

    public void HarvestYield(int yield)
    {
        if (yield == 4)
        {
            saveManager.SetFieldData(nameof(saveManager.userData.food), 100, true);
            return;
        }

        yield = yield switch
        {
            1 => 2,
            2 => 4,
            3 => 7,
            _ => 0
        };

        int randNum = Random.Range(0, 100);
        if (randNum < 40)
        {
            saveManager.SetFieldData(nameof(saveManager.userData.food), yield * 3, true);
        }
        else if (randNum < 75)
        {
            saveManager.SetFieldData(nameof(saveManager.userData.food), yield * 5, true);
        }
        else
        {
            saveManager.SetFieldData(nameof(saveManager.userData.food), yield * 8, true);
        }
    }

    public void SaveFarmData()
    {
        FieldData[] temp = new FieldData[6];
        for (int i = 0; i < temp.Length; i++)
        {
            FieldData tempItem = fieldSlots[i].fieldData;
            temp[i] = tempItem;
        }
        saveManager.SetFieldData(nameof(userData.fieldDatas), temp);
    }

    public void SetAllPriceTxts()
    {
        for (int i = 0; i < 6; i++)
        {
            fieldSlots[i].SetPriceTxt();
        }
    }
}