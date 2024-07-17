using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonTab : UIBase
{
    [Header("UI")]
    [SerializeField]
    private Toggle prossessToggle;

    [SerializeField]
    private TextMeshProUGUI seedCountTxt;


    //픽셀몬 슬롯 클릭시 활성화 오브젝트
    public RectTransform clickPopUp;
    [SerializeField]
    private Button equipBtn;
    [SerializeField]
    private Button infoBtn;
    public GameObject equipProcessPanel;
    [SerializeField]
    private int choiceId;
    [SerializeField]
    private TextMeshProUGUI equipTxt;
    private string equip = "장착하기";
    private string unEquip = "해제하기";
    public TabState tabState;


    [SerializeField]
    private PixelmonPopUP infoPopUp;
    //픽셀몬 슬롯 프리팹
    [SerializeField]
    private PixelmonSlot slotPrefab;
    //전체 슬롯 부모 오브젝트 위치
    [SerializeField]
    private Transform contentTr;

    #region Info
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
    private PixelmonEquipSlot[] equipData = new PixelmonEquipSlot[5];
    #endregion
    // Start is called before the first frame update
    async void Start()
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
            slot.InitSlot(this, data);
            allData.Add(slot);
        }

        for (int i = 0; i < equipData.Length; i++)
        {
            if (inven.userData.equipedPixelmons.Length > i)
            {
                equipData[i].pixelmonData = inven.userData.equipedPixelmons[i];
            }
            equipData[i].pixelmontab = this;
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

    public void OnClickSlot(int index, RectTransform rectTr)
    {
        choiceId = index;
        clickPopUp.gameObject.SetActive(true);
        clickPopUp.position = rectTr.position + Vector3.down * 130;
        equipProcessPanel.gameObject.SetActive(true);
        equipProcessPanel.gameObject.transform.SetSiblingIndex(6);
        equipProcessPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        if (allData[index].pixelmonData.isEquiped)
            equipTxt.text = unEquip;
        else
            equipTxt.text = equip;
    }

    public void OnHideOverlay()
    {
        tabState = TabState.Normal;
        clickPopUp.gameObject.SetActive(false);
        equipProcessPanel.gameObject.SetActive(false);
    }
    
    public void Possess(int index)
    {
        allData[index].pixelmonData.isPossessed = true;
    }

    public void OnEquip()
    {
        tabState = TabState.Equip;
        clickPopUp.gameObject.SetActive(false);
        equipProcessPanel.gameObject.transform.SetSiblingIndex(5);
        equipProcessPanel.GetComponent<Image>().color = new Color32(0, 0, 0, 90);
    }


    public void EquipedPixelmon(int slotIndex)
    {
        if (allData[choiceId].pixelmonData.isEquiped)
        {
            for(int i = 0; i < equipData.Length; i++) 
            {
                if (equipData[i].pixelmonData == allData[choiceId].pixelmonData)
                {
                    RemoveEquipSlot(i);
                    break;
                }
            }
        }
        else
        {
            equipData[slotIndex].Equip(allData[choiceId].pixelmonData);
            equipProcessPanel.gameObject.SetActive(false);
        }
        tabState = TabState.Normal;
    }

    public void RemoveEquipSlot(int slotIndex)
    {
        equipData[slotIndex].pixelmonData.isEquiped = false;
        equipData[slotIndex].UnEquip();
        equipData[slotIndex] = null;
    }

    public void OnInfoPopUp()
    {
        infoPopUp.gameObject.SetActive(true);
        infoPopUp.ShowPopUp(choiceId);
    }
}

public enum TabState
{
    Normal,
    Equip
}