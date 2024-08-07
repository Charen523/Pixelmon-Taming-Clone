using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillGacha : MonoBehaviour
{
    [SerializeField] private SkillGachaBtn OneBtn;
    [SerializeField] private SkillGachaBtn TenBtn;
    [SerializeField] private SkillGachaBtn ThirtyBtn;

    private UserData userData => SaveManager.Instance.userData;
    private UISkillGachaPopup skillGachaPopup;
    private MyAtvData randSkill;
    private int oneCostTicket = 1; // 1티켓 == 1회 뽑기
    private int oneCostDia = 100;  // 100다이아 == 1회 뽑기

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
        SetBtnCostUIs();
    }

    #region UI
    private void SetBtnInteractable()
    {
        OneBtn.Btn.interactable = false;
        TenBtn.Btn.interactable = false;
        ThirtyBtn.Btn.interactable = false;

        UpdateButnInteractable(OneBtn.Btn, 1);
        UpdateButnInteractable(TenBtn.Btn, 10);
        UpdateButnInteractable(ThirtyBtn.Btn, 30);
    }

    private void UpdateButnInteractable(Button button, int requiredAmount)
    {
        if ((userData.skillTicket / oneCostTicket + userData.diamond / oneCostDia) >= requiredAmount)
            button.interactable = true;
    }

    private void UpdateCostUI(DirtyUI dirtyUI)
    {
        if (dirtyUI == DirtyUI.Diamond || dirtyUI == DirtyUI.SkillTicket)
            SetBtnCostUIs();
    }
    private void SetBtnCostUIs()
    {
        SetBtnCostUI(OneBtn, 1);
        SetBtnCostUI(TenBtn, 10);
        SetBtnCostUI(ThirtyBtn, 30);
    }
    private void SetBtnCostUI(SkillGachaBtn btn, int multiplier)
    {
        int totalCostTicket = oneCostTicket * multiplier;
        int totalCostDia = oneCostDia * multiplier;

        if (userData.skillTicket >= totalCostTicket)
        {
            btn.SetTicket(totalCostTicket);
            btn.HideDia();
        }
        else if (multiplier > 1 && userData.skillTicket >= oneCostTicket)
        {
            int neededTickets = userData.skillTicket / oneCostTicket;
            int reaminingCost = totalCostTicket - neededTickets;
            int neededDiamonds = (reaminingCost / oneCostTicket) * oneCostDia;

            btn.SetAllCost(neededTickets, neededDiamonds);
        }
        else
        {
            btn.SetDia(totalCostDia);
            btn.HideTicket();
        }
    }
    #endregion

    public void OnClickBtn(int multiplier)
    {
        int totalCostTicket = oneCostTicket * multiplier;
        int totalCostDia = oneCostDia * multiplier;

        if (userData.skillTicket >= totalCostTicket) // 티켓 먼저 소모
        {
            SaveManager.Instance.SetFieldData(nameof(userData.skillTicket), -totalCostTicket, true);
        }
        else if (multiplier > 1 && userData.skillTicket >= oneCostTicket)
        {
            int remainingTickets = userData.skillTicket % oneCostTicket;
            int reaminingCost = totalCostTicket - userData.skillTicket / oneCostTicket;
            int neededDiamonds = (reaminingCost / oneCostTicket) * oneCostDia;

            SaveManager.Instance.SetFieldData(nameof(userData.diamond), -neededDiamonds, true);
            SaveManager.Instance.SetFieldData(nameof(userData.skillTicket), remainingTickets);
        }
        else
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
            Debug.Log("스킬 id : " + id);
        }

        skillGachaPopup.SetActive(true);
        skillGachaPopup.SetPopup(count, resultDatas);
    }
}
