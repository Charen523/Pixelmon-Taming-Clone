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

    Coroutine growingCoroutine;
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
                priceTxt.gameObject.SetActive(true);
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
                priceTxt.gameObject.SetActive(true);
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
                break;

            case FieldState.Seeded: //작물이 심긴 밭.
                /*Btn Settings*/
                pxmBtn.interactable = false;
                FieldBtn.interactable = false;
                /*UI Settings*/
                currentSprite.sprite = fieldStatusImgs[2];
                FieldIcon.sprite = Icons[3];
                /*TMP Settings*/
                if (growingCoroutine != null)
                {
                    StopCoroutine(growingCoroutine);
                }
                growingCoroutine = StartCoroutine(plantGrowing()); //남은 시간
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
                /*TMP Settings*/
                if (growingCoroutine != null)
                {
                    StopCoroutine(growingCoroutine);
                }
                if (timeTxt.gameObject.activeSelf)
                {
                    timeTxt.gameObject.SetActive(false);
                }
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
            fieldData.leftTime = fieldData.yieldClass * 2 * 3600f;
            fieldData.lastSaveTime = DateTime.Now.ToString();
            CurrentFieldState = FieldState.Seeded;
        }
        else
        {
            //TODO: 씨앗 없음을 알리는 popup
            Debug.LogWarning("씨앗 없음 Popup");
        }
    }

    private void OnHarvestFieldClicked()
    {
        int yield;
        switch (fieldData.yieldClass)
        {
            case 1:
                yield = 1;
                break;
            case 2:
                yield = 3;
                break;
            case 3:
                yield = 10;
                break;
            default:
                Debug.LogError("Harvest Class 값이 이상함");
                yield = 0;
                break;
        }

        farmTab.HarvestYield(yield);
        CurrentFieldState = FieldState.Empty;
    }

    private void CalculatePassiveEffect()
    {
        fieldData.yieldClass = RandomNumGenerator();
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
        DateTime lastTime = DateTime.Parse(fieldData.lastSaveTime);
        TimeSpan elapsed = DateTime.Now - lastTime;
        fieldData.leftTime -= (float)elapsed.TotalSeconds;

        if (fieldData.leftTime <= 0)
        {
            fieldData.currentFieldState = FieldState.Harvest;
        }
    }
    private int RandomNumGenerator()
    {
        int randomNum = UnityEngine.Random.Range(0, 10);

        if (randomNum < 6)
        {
            return 1;
        }
        else if (randomNum < 9)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }
}