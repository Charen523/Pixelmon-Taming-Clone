using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Threading;
using System;

public class FieldSlot : SerializedMonoBehaviour
{
    /*FarmTab에서 Awake 메서드로 초기화되는 변수들*/
    public FarmTab farmTab;
    public int myIndex;
    public FieldData fieldData;

    #region properties
    public FieldState CurrentFieldState 
    {
        get => fieldData.currentFieldState;
        set
        {
            if (fieldData.currentFieldState != value)
            {
                fieldData.currentFieldState = value;
                CurrentFieldAction(value);
            }
        }
    }
    #endregion

    #region UI
    [SerializeField] private Button pxmBtn;
    [SerializeField] private Button FieldBtn;
    
    [SerializeField] private Sprite[] fieldStatusImgs;
    [SerializeField] private Image currentSprite;

    [SerializeField] private Sprite[] Icons;
    [SerializeField] private Image FieldIcon;

    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;
    #endregion

    public int harvestHour;
    public int yield; //수확량
    [SerializeField] private int price; //밭 가격

    private void OnEnable()
    {
        if (fieldData.currentFieldState == FieldState.Seeded)
        {
            CalculateRemainingTime(); 
        }

        CurrentFieldState = fieldData.currentFieldState;
        CurrentFieldAction(CurrentFieldState);
    }
    private void CurrentFieldAction(FieldState state)
    {
        switch(state)
        {
            case FieldState.Locked: //잠김.
                /*Btn Settings*/
                pxmBtn.interactable = false;
                FieldBtn.interactable = false;
                /*UI Settings*/
                currentSprite.sprite = fieldStatusImgs[0];
                FieldIcon.sprite = Icons[0];
                break;

            case FieldState.Buyable: //구매가능
                /*Btn Settings*/
                pxmBtn.interactable = true;
                FieldBtn.interactable = true;
                FieldBtn.onClick.AddListener(OnBuyFieldClicked);
                /*UI Settings*/
                currentSprite.sprite = fieldStatusImgs[1];
                FieldIcon.sprite = Icons[1];
                /*TMP Settings*/
                priceTxt.text = $"가격: {price}다이아";
                break;

            case FieldState.Empty: //빈 밭.
                /*Btn Settings*/
                FieldBtn.interactable = true;
                FieldBtn.onClick.RemoveAllListeners();
                FieldBtn.onClick.AddListener(OnSeedFieldClicked);
                /*UI Settings*/
                currentSprite.sprite = fieldStatusImgs[1];
                FieldIcon.sprite = Icons[2];
                /*TMP Settings*/
                priceTxt.gameObject.SetActive(false);
                break;

            case FieldState.Seeded: //작물이 심긴 밭.
                /*Btn Settings*/
                pxmBtn.interactable = false;
                FieldBtn.interactable = false;
                /*UI Settings*/
                currentSprite.sprite = fieldStatusImgs[2];
                FieldIcon.sprite = Icons[3];
                /*TMP Settings*/
                StartCoroutine(plantGrowing()); //남은 시간
                break;

            case FieldState.Harvest: //수확 준비된 밭.
                /*Btn Settings*/
                pxmBtn.interactable = true;
                FieldBtn.interactable = true;
                FieldBtn.onClick.RemoveAllListeners();
                FieldBtn.onClick.AddListener(OnHarvestFieldClicked);
                /*UI Settings*/
                currentSprite.sprite = fieldStatusImgs[3];
                FieldIcon.sprite = Icons[4];
                break;

            default:
                Debug.LogError($"{myIndex}번 밭에서 잘못된 FieldState.");
                break;
        }
    }

    private void OnBuyFieldClicked()
    {
        //TODO: 구매 팝업 띄우는 것으로 대체하기
        if (price <= InventoryManager.Instance.userData.diamond)
        {
            InventoryManager.Instance.SetDeltaData("diamond", -price);
            CurrentFieldState = FieldState.Empty;
        }
        else
        {
            //TODO: 다이아 없음을 알리는 popup
            Debug.LogWarning("다이아 없음 Popup");
        }
    }

    private void OnSeedFieldClicked()
    {
        if (farmTab.PlantSeed())
        {
            CalculatePassiveEffect();
            CurrentFieldState = FieldState.Seeded;
            fieldData.leftTime = harvestHour * 3600f;
        }
        else
        {
            //TODO: 씨앗 없음을 알리는 popup
            Debug.LogWarning("씨앗 없음 Popup");
        }
    }

    private void OnHarvestFieldClicked()
    {
        farmTab.HarvestYield(yield);
    }

    private void CalculatePassiveEffect()
    {
        if (fieldData.farmer == null)
        {
            harvestHour = 1; //2, 4, 6 중 랜덤.
            yield = 1; //1, 3, 10 중 랜덤.
        }
    }

    private IEnumerator plantGrowing()
    {
        timeTxt.gameObject.SetActive(true);
        while (fieldData.leftTime > 0)
        {
            fieldData.leftTime -= Time.deltaTime;
            int hours = Mathf.FloorToInt(fieldData.leftTime / 3600f);
            int minutes = Mathf.FloorToInt((fieldData.leftTime % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(fieldData.leftTime % 60f);
            timeTxt.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            yield return null;
        }
        timeTxt.gameObject.SetActive(false);
        CurrentFieldState = FieldState.Harvest;
    }

    private void CalculateRemainingTime()
    {
        TimeSpan elapsed = DateTime.Now - fieldData.lastSaveTime;
        fieldData.leftTime -= (float)elapsed.TotalSeconds;

        if (fieldData.leftTime <= 0)
        {
            fieldData.currentFieldState = FieldState.Harvest;
        }
    }
    private int RandomNumGenerator()
    {
        return UnityEngine.Random.Range(0, 3);
    }
}

//    public void Set()
//    {
//        icon.gameObject.SetActive(true);
//        icon.sprite = item.icon;
//        quatityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

//        if (outline != null)
//        {
//            outline.enabled = equipped;
//        }
//    }

//    public void Clear()
//    {
//        item = null;
//        icon.gameObject.SetActive(false);
//        quatityText.text = string.Empty;
//    }

//    public void OnClickButton()
//    {
//        inventory.SelectItem(index);
//    }
//}