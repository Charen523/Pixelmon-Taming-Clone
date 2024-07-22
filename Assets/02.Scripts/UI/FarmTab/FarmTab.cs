using TMPro;
using UnityEngine;

public class FarmTab : UIBase
{
    private SaveManager invenManager;

    public FieldSlot[] fieldSlots;
    [SerializeField] private Transform fieldsParent;

    #region Inventory Data
    int seedCount => SaveManager.Instance.userData.seed;
    int foodCount => SaveManager.Instance.userData.food;
    #endregion

    #region UI
    private UIBase farmPxmPopup;

    [SerializeField] private TextMeshProUGUI seedTxt;
    [SerializeField] private TextMeshProUGUI foodTxt;
    #endregion

    private bool isAwakeEnabled;

    private async void Awake()
    {
        isAwakeEnabled = false;

        invenManager = SaveManager.Instance;
        farmPxmPopup = await UIManager.Show<UIFarmPixelmonPopup>();

        fieldSlots = new FieldSlot[fieldsParent.childCount];

        for (int i = 0; i < fieldSlots.Length; i++)
        {
            fieldSlots[i] = fieldsParent.GetChild(i).GetComponent<FieldSlot>();
            fieldSlots[i].farmTab = this;
            fieldSlots[i].myIndex = i;
            fieldSlots[i].fieldData = invenManager.userData.fieldDatas[i];
        }

        isAwakeEnabled = true;
    }

    private void OnEnable()
    {
        /*상단 UI 초기화*/
        seedTxt.text = seedCount.ToString();
        foodTxt.text = foodCount.ToString();  

        if (isAwakeEnabled)
        {
            for (int i = 0; i < fieldSlots.Length; i++)
            {
                fieldSlots[i].gameObject.SetActive(true);
            }
        }
        
        //밭 해금조건 예시
        //if (fieldSlots[2].CurrentFieldState == FieldState.Locked && invenManager.userData.lv >= 99)
        //{
        //    fieldSlots[2].CurrentFieldState = FieldState.Buyable;
        //}
    }

    private void OnDisable()
    {
        if (isAwakeEnabled)
        {
            SaveFieldData();
        }
    }

    private void OnApplicationQuit()
    {
        if (isAwakeEnabled)
        {
            SaveFieldData();
        }
    }

    public void ShowEquipPixelmon()
    {
        farmPxmPopup.SetActive(true);
    }

    public bool PlantSeed()
    {
        if (seedCount == 0)
        {
            return false;
        }
        else
        {
            invenManager.SetDeltaData(nameof(invenManager.userData.seed), -1);
            seedTxt.text = seedCount.ToString();
            return true;
        }
    }

    public void HarvestYield(int yield)
    {
        invenManager.SetDeltaData("petFood", yield);
        foodTxt.text = invenManager.userData.food.ToString();
    }

    public void SetFieldPixelmon(int index)
    {

    }

    private void SaveFieldData()
    {
        FieldData[] temp = new FieldData[fieldsParent.childCount];
        for (int i = 0; i < temp.Length; i++)
        {
            FieldData tempItem = fieldSlots[i].fieldData;
            fieldSlots[i].gameObject.SetActive(false);

            if (tempItem.currentFieldState == FieldState.Seeded)
            {
                tempItem.lastSaveTime = System.DateTime.Now.ToString();
            }

            temp[i] = tempItem;
        }

        invenManager.SetData("fieldDatas", temp);
    }
}