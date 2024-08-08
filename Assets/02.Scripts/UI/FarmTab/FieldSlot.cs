using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;
using UnityEditor.Build.Pipeline;

public class FieldSlot : MonoBehaviour
{
    private SaveManager saveManager;
    private UserData userData;

    public FarmTab farmTab;
    public int slotIndex;
    public FieldData fieldData;

    [SerializeField] private int price;
    [SerializeField] private float passTime;

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
    [SerializeField] private Button buyBtn;
    [SerializeField] private Button seedBtn;
    [SerializeField] private Button harvestBtn;
    
    [SerializeField] private Sprite[] plantImgs;
    [SerializeField] private Image curSprite;

    [SerializeField] private Slider timeSldr;
    [SerializeField] private TextMeshProUGUI timeTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;
    #endregion

    Coroutine growingCoroutine;

    private void Awake()
    {
        saveManager = SaveManager.Instance;
        userData = saveManager.userData;
    }

    public void CurrentFieldAction(FieldState state)
    {
        switch (state)
        {
            case FieldState.Locked: //잠김.
                break;

            case FieldState.Buyable: //구매가능
                /*Btn Settings*/
                buyBtn.gameObject.SetActive(true);
                /*UI Settings*/
                break;

            case FieldState.Empty: //빈 밭.
                /*Btn Settings*/
                buyBtn.transform.parent.gameObject.SetActive(false);
                seedBtn.gameObject.SetActive(true);
                harvestBtn.gameObject.SetActive(false);
                /*UI Settings*/
                curSprite.gameObject.SetActive(false);
                break;

            case FieldState.Seeded: //작물이 심긴 밭.
                /*Btn Settings*/
                buyBtn.transform.parent.gameObject.SetActive(false);
                seedBtn.gameObject.SetActive(false);
                /*UI Settings*/
                curSprite.gameObject.SetActive(true);
                curSprite.sprite = plantImgs[0];
                timeSldr.gameObject.SetActive(true);
                /*TMP Settings*/
                if (growingCoroutine != null)
                {
                    StopCoroutine(growingCoroutine);
                }
                growingCoroutine = StartCoroutine(plantGrowing()); //남은 시간
                break;

            case FieldState.Harvest: //수확 준비된 밭.
                /*Btn Settings*/
                buyBtn.transform.parent.gameObject.SetActive(false);
                harvestBtn.gameObject.SetActive(true);
                /*UI Settings*/
                curSprite.sprite = plantImgs[fieldData.cropClass];
                timeSldr.gameObject.SetActive(false);
                /*TMP Settings*/
                if (growingCoroutine != null)
                {
                    StopCoroutine(growingCoroutine);
                }
                break;
        }
    }

    public void OnBuyBtn()
    {
        if (slotIndex < 4)
        {
            if (price <= userData.gold)
            {
                saveManager.SetFieldData(nameof(userData.gold), -price, true);
            }
            else
            {
                Debug.LogWarning("돈 없음 popup");
                return;
            }
        }
        else
        {
            if (price <= userData.diamond)
            {
                saveManager.SetFieldData(nameof(userData.diamond), -price, true);
            }
            else
            {
                Debug.LogWarning("다이아 없음 popup");
                return;
            }
        }
        CurrentFieldState = FieldState.Empty;
        buyBtn.transform.parent.gameObject.SetActive(false);
        farmTab.SaveFarmData();
        farmTab.UnlockNextField(slotIndex);
    }

    public void OnSeedBtn()
    {
        if (farmTab.PlantSeed())
        {
            RandomCrop();
            if (fieldData.cropClass == 4) fieldData.leftTime = 3 * 2 * 3600f;
            else fieldData.leftTime = fieldData.cropClass * 2 * 3600f;
            passTime = fieldData.leftTime;
            fieldData.startTime = DateTime.Now.ToString();
            CurrentFieldState = FieldState.Seeded;
        }
        farmTab.SaveFarmData();
    }

    public void OnHarvestBtn()
    {
        int yield = 0;
        switch (fieldData.cropClass)
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
        }
        farmTab.HarvestYield(yield);
        CurrentFieldState = FieldState.Empty;
        farmTab.SaveFarmData();
    }

    private void SetPriceTxt()
    {
        if (CurrentFieldState == FieldState.Locked)
        {
            priceTxt.color = Color.red;
            return;
        }

        if (slotIndex < 4 && price > userData.gold)
        {
            priceTxt.color = Color.red;
            return;
        }

        if (slotIndex > 3 &&  price > userData.diamond)
        {
            priceTxt.color = Color.red;
            return;
        }

        priceTxt.color = Color.white;
    }

    private void RandomCrop()
    {
        int randNum = UnityEngine.Random.Range(0, 100);
        if (randNum < 59)
        {
            fieldData.cropClass = 1;
        }
        else if (randNum < 92)
        {
            fieldData.cropClass = 2;
        }
        else if (randNum < 99)
        {
            fieldData.cropClass = 3;
        }
        else
        {
            fieldData.cropClass = 4;
        }
    }

    private IEnumerator plantGrowing()
    {
        timeTxt.gameObject.SetActive(true);
        while (passTime > 0)
        {
            passTime -= Time.deltaTime;
            int hours = Mathf.FloorToInt(passTime / 3600f);
            int minutes = Mathf.FloorToInt((passTime % 3600f) / 60f);
            int seconds = Mathf.FloorToInt(passTime % 60f);
            timeTxt.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            timeSldr.value = passTime / fieldData.leftTime;
            yield return null;
        }
        timeTxt.gameObject.SetActive(false);
        CurrentFieldState = FieldState.Harvest;
    }

    public void CalculateRemainingTime()
    {
        DateTime time = DateTime.Parse(fieldData.startTime);
        TimeSpan elapsed = DateTime.Now - time;
        passTime = fieldData.leftTime - (float)elapsed.TotalSeconds;

        if (passTime <= 0)
        {
            fieldData.currentFieldState = FieldState.Harvest;
        }
    }
}