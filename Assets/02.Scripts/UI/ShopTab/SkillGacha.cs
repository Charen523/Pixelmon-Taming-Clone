using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillGacha : MonoBehaviour
{
    [SerializeField] private Button OneBtn;
    [SerializeField] private Button TenBtn;
    [SerializeField] private Button ThirtyBtn;

    private UserData userData => SaveManager.Instance.userData;
    private UISkillGachaPopup skillGachaPopup;
    private MyAtvData randSkill = new MyAtvData();
    private int oneCostTicket = 1; // 1티켓 == 1회 뽑기
    private int oneCostDia = 100;  // 100다이아 == 1회 뽑기

    public async void Awake()
    {
        skillGachaPopup = await UIManager.Show<UISkillGachaPopup>();
    }

    public void SetSkillGacha()
    {
        SetBtnInteractable();
        SetBtnCostUI(1);
        SetBtnCostUI(10);
        SetBtnCostUI(30);
    }

    #region UI
    private void SetBtnInteractable()
    {
        OneBtn.interactable = false;
        TenBtn.interactable = false;
        ThirtyBtn.interactable = false;

        if (userData.skillTicket / oneCostTicket + userData.diamond / oneCostDia >= 1)
            OneBtn.interactable = true;
        if (userData.skillTicket / oneCostTicket + userData.diamond / oneCostDia >= 10)
            TenBtn.interactable = true;
        if (userData.skillTicket / oneCostTicket + userData.diamond / oneCostDia >= 30)
            ThirtyBtn.interactable = true;
    }

    private void SetBtnCostUI(int multiplier)
    {
        int totalCost = oneCostTicket * multiplier;

        if (userData.skillTicket >= totalCost)
        {
        }
        else if (multiplier > 1 && userData.skillTicket >= oneCostTicket)
        {
        }
        else
        {
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
