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
    public bool isOnToggle;

    [SerializeField]
    private TextMeshProUGUI seedCountTxt;

    [Header("Info")]
    [SerializeField]
    private InventoryManager inven;
    //전체 픽셀몬 정보
    [SerializeField]
    private List<PixelmonSlot> allData;
    //미보유 픽셀몬 정보
    [SerializeField]
    private List<GameObject> noneData;
    //보유한 픽셀몬 정보
    [SerializeField]
    private List<PixelmonSlot> possessData;
    //편성된 픽셀몬 정보
    [SerializeField]
    private PixelmonSlot[] composedData = new PixelmonSlot[5];

    // Start is called before the first frame update
    void Start()
    {
        inven = InventoryManager.Instance;
    }

    public void InitToggle()
    {
        CheckedDate();
        seedCountTxt.text = inven.userData.petFood.ToString();
    }

    public void InitInfo()
    {

    }

    public void SetPetfoodCount(int count)
    {
        inven.SetAddData(nameof(inven.userData.petFood), count);
        seedCountTxt.text = inven.userData.petFood.ToString();
    }
    public void CheckedDate()
    {
        foreach (var data in allData)
        {
            if (data.pixelmonData.isPossess)
            {
                possessData.Add(data);
                if(data.lockIcon.activeSelf) data.lockIcon.SetActive(false);
            }
            else
            {
                noneData.Add(data.gameObject);
            }
        }
        OnProssessionToggle();
    }

    public void OnProssessionToggle()
    {
        foreach (GameObject data in noneData)
        {
            data.gameObject.SetActive(!isOnToggle);
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
    
    public void Possess()
    {
        
    }

    public void EnrolledPixelmon()
    {

    }
    public void EquipSkill()
    {

    }
}
