using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonTab : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Toggle prossessToggle;

    [SerializeField]
    private TextMeshProUGUI seedCountTxt;

    [SerializeField]
    private PixelmonSlot slotPrefab;
    [SerializeField]
    private Transform contentTr;
    [Header("Info")]
    [SerializeField]
    private InventoryManager inven;
    [SerializeField]
    private DataManager dataManager;
    //전체 픽셀몬 정보
    public List<PixelmonSlot> allData = new List<PixelmonSlot>();
    //미보유 픽셀몬 정보
    [SerializeField]
    private List<PixelmonSlot> noneData = new List<PixelmonSlot>();
    //보유한 픽셀몬 정보
    [SerializeField]
    private List<PixelmonSlot> possessData = new List<PixelmonSlot>();
    //편성된 픽셀몬 정보
    [SerializeField]
    private PixelmonSlot[] composedData = new PixelmonSlot[5];

    // Start is called before the first frame update
    void Start()
    {
        inven = InventoryManager.Instance;
        dataManager = DataManager.Instance;
        InitTab();
    }

    public void InitTab()
    {
        foreach (var data in dataManager.pixelmonData.data)
        {
            PixelmonSlot slot = Instantiate(slotPrefab, contentTr);
            slot.pixelmonData = data;
            allData.Add(slot);
        }
        for (int i = 0; i < composedData.Length; i++)
        {
            composedData[i].pixelmonData = inven.userData.composedPixelmons[i];
        }
        CheckedData();
        seedCountTxt.text = inven.userData.petFood.ToString();
    }

    public void InitInfo()
    {

    }

    public void SetPetfoodCount(int count)
    {
        inven.SetDeltaData(nameof(inven.userData.petFood), count);
        seedCountTxt.text = inven.userData.petFood.ToString();
    }

    public void CheckedData()
    {
        //findall 로 널일떄만
        possessData = allData.FindAll(obj => obj.isPossessed);
        noneData = allData.FindAll(obj => !obj.isPossessed);
        OnProssessionToggle();
    }

    public void OnProssessionToggle()
    {
        foreach (PixelmonSlot data in noneData)
        {
            data.gameObject.SetActive(!prossessToggle.isOn);
        }
    }

    public void Feeding()
    {

    }

    public void AutoProssess()
    {

    }

    public void AutoCompose()
    {

    }

    public void ChoicePixelmon()
    {

    }
    
    public void Possess(int index)
    {
        allData[index].pixelmonData.isPossessed = true;
    }

    public void EnrolledPixelmon(int possessIndex, PixelmonData pixelData)
    {
        if (pixelData.isPossessed)
        {
            for(int i = 0; i < possessData.Count; i++) 
            {
                if (possessData[i].pixelmonData == pixelData)
                {
                    RemovePossessSlot(i);
                    break;
                }
            }                           
        }
        possessData[possessIndex].InitSlot(pixelData);
    }

    public void RemovePossessSlot(int possessIndex)
    {
        possessData[possessIndex].RemoveInfo();
        possessData[possessIndex] = null;
    }
}
