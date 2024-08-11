using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillGacha : MonoBehaviour
{
    [SerializeField] private SkillGachaBtn FiveBtn;
    [SerializeField] private SkillGachaBtn FifteenBtn;
    [SerializeField] private SkillGachaBtn ThirtyBtn;
    [SerializeField] private TextMeshProUGUI TicketCostTxt;

    private UserData userData => SaveManager.Instance.userData;
    private UISkillGachaPopup skillGachaPopup;
    private MyAtvData randSkill;
    private int oneCostTicket = 1; // 1티켓 == 1회 뽑기
    private int oneCostDia = 200;  // 200다이아 == 1회 뽑기

    private async void Awake()
    {
        skillGachaPopup = await UIManager.Show<UISkillGachaPopup>();
    }

    private void Start()
    {
        UIManager.Instance.UpdateUI += UpdateCostUI;
    }

    public void SetSkillGacha()
    {
        SetBtnInteractable();
    }

    #region UI
    private void SetBtnInteractable()
    {
        UpdateButnInteractable(FiveBtn.Btn, 5);
        UpdateButnInteractable(FifteenBtn.Btn, 15);
        UpdateButnInteractable(ThirtyBtn.Btn, 30);
    }

    public void UpdateButnInteractable(Button button, int requiredAmount)
    {
        if ((userData.skillTicket >= requiredAmount * oneCostTicket) 
            || (userData.diamond >= requiredAmount * oneCostDia))
            button.interactable = true;
        else
            button.interactable = false;
    }

    private void UpdateCostUI(DirtyUI dirtyUI)
    {
        if (dirtyUI == DirtyUI.Diamond || dirtyUI == DirtyUI.SkillTicket)
        {
            SetBtnInteractable();
            TicketCostTxt.text = userData.skillTicket.ToString();
        }            
    }
    #endregion

    public void OnClickBtn(int multiplier)
    {
        int totalCostTicket = oneCostTicket * multiplier;
        int totalCostDia = oneCostDia * multiplier;

        if (userData.skillTicket >= totalCostTicket) // 티켓
        {
            SaveManager.Instance.SetFieldData(nameof(userData.skillTicket), -totalCostTicket, true);
        }
        else // 다이아
        {
            SaveManager.Instance.SetFieldData(nameof(userData.diamond), -totalCostDia, true);
        }

        Gacha(multiplier);
    }

    private void Gacha(int count)
    {     
        ActiveData[] resultDatas = new ActiveData[count];       
        var ownedSkills = userData.ownedSkills;
        int id;

        for (int i = 0; i < count; i++)
        {
            randSkill = new MyAtvData();
            resultDatas[i] = new ActiveData();

            id = UnityEngine.Random.Range(0, DataManager.Instance.activeData.data.Count);
            resultDatas[i] = DataManager.Instance.activeData.data[id];

            if (SkillManager.Instance.skillTab.allData[id].myAtvData.isOwned) // 이미 있는 스킬
            {
                int index = SkillManager.Instance.skillTab.allData[id].atvData.dataIndex;
                if(index != -1)
                    SaveManager.Instance.UpdateSkillData(index, "evolvedCount", ownedSkills[index].evolvedCount + 1);
            }
            else // 새롭게 뽑은 스킬
            {
                randSkill.rcode = resultDatas[i].rcode;
                randSkill.id = resultDatas[i].id;
                randSkill.isOwned = true;
                ownedSkills.Add(randSkill);
                SaveManager.Instance.SetFieldData(nameof(userData.ownedSkills), ownedSkills);
            }
            SkillManager.Instance.AddSkill(id);
        }

        skillGachaPopup.SetActive(true);
        skillGachaPopup.SetPopup(count, resultDatas, this);
    }
}
