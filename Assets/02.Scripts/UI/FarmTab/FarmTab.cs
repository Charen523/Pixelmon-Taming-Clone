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

    private bool isAwakeEnabled;
    private void Awake()
    {
        isAwakeEnabled = false;
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
        UIManager.Instance.UpdateUI += UpdateFieldUI;

        for (int i = 0; i < fieldSlots.Length; i++)
        {
            fieldSlots[i].farmTab = this;
            fieldSlots[i].myIndex = i;
            fieldSlots[i].fieldData = userData.fieldDatas[i];
        }
        seedTxt.text = seedCount.ToString();
        foodTxt.text = foodCount.ToString();
        isAwakeEnabled = true;
    }

    private void OnEnable()
    {
        if (isAwakeEnabled)
        {
            seedTxt.text = seedCount.ToString();
            foodTxt.text = foodCount.ToString();

            for (int i = 0; i < fieldSlots.Length; i++)
            {
                fieldSlots[i].CurrentFieldAction(fieldSlots[i].CurrentFieldState);
                if (fieldSlots[i].CurrentFieldState == FieldState.Seeded)
                {
                    fieldSlots[i].CalculateRemainingTime();
                }
            }
        }
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
        saveManager.SetFieldData(nameof(saveManager.userData.food), yield, true);
    }

    public void SaveFarmData()
    {
        FieldData[] temp = new FieldData[6];
        for (int i = 0; i < temp.Length; i++)
        {
            FieldData tempItem = fieldSlots[i].fieldData;
            fieldSlots[i].gameObject.SetActive(false);
            temp[i] = tempItem;
        }
        saveManager.SetFieldData(nameof(userData.fieldDatas), temp);
    }
}