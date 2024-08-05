using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillGacha : MonoBehaviour
{
    [SerializeField] private Button OneBtn;
    [SerializeField] private Button TenBtn;
    [SerializeField] private Button ThirtyBtn;

    private UserData userData => SaveManager.Instance.userData;
    private int oneCostTicket = 1; // 1티켓 == 1회 뽑기
    private int oneCostDia = 100;  // 100다이아 == 1회 뽑기

    public void SetSkillGacha()
    {
        SetBtnInteractable();
        SetBtnCostUIs();
    }

    #region UI
    private void SetBtnInteractable()
    {
        OneBtn.interactable = false;
        TenBtn.interactable = false;
        ThirtyBtn.interactable = false;

        if (userData.skillTicket >= oneCostTicket || userData.diamond >= oneCostDia)
            OneBtn.interactable = true;
        if (userData.skillTicket >= oneCostTicket * 10 || userData.diamond >= oneCostDia * 10)
            TenBtn.interactable = true;
        if (userData.skillTicket >= oneCostTicket * 30 || userData.diamond >= oneCostDia * 30)
            ThirtyBtn.interactable = true;
    }

    private void SetBtnCostUIs()
    {
        SetOneBtnCostUI();
        SetTenBtnCostUI();
        SetThirtyBtnCostUI();
    }

    private void SetOneBtnCostUI()
    {
        if (userData.skillTicket >= oneCostTicket)
        {

        }
        else
        {

        }
    }

    private void SetTenBtnCostUI()
    {
        if (userData.skillTicket >= oneCostTicket * 10)
        {

        }
        else if (userData.skillTicket >= oneCostTicket)
        {

        }
        else
        {

        }
    }

    private void SetThirtyBtnCostUI()
    {
        if (userData.skillTicket >= oneCostTicket * 30)
        {

        }
        else if (userData.skillTicket >= oneCostTicket)
        {

        }
        else
        {

        }
    }
    #endregion

    #region OnClickBtns
    public void OnClickOneBtn()
    {
        if (userData.skillTicket >= oneCostTicket) // 티켓 먼저 소모
        {
            SaveManager.Instance.SetFieldData(nameof(userData.skillTicket), -oneCostTicket, true);
        }
        else
        {
            SaveManager.Instance.SetFieldData(nameof(userData.diamond), -oneCostDia, true);
        }
        Gacha(1);
    }
    public void OnClickTenBtn()
    {
        if (userData.skillTicket >= oneCostTicket * 10) // 티켓 먼저 소모
        {
            SaveManager.Instance.SetFieldData(nameof(userData.skillTicket), -oneCostTicket * 10, true);
        }
        else if (userData.skillTicket >= oneCostTicket)
        {
            SaveManager.Instance.SetFieldData(nameof(userData.diamond), -(oneCostDia * (10 - userData.skillTicket / oneCostTicket)), true);
            SaveManager.Instance.SetFieldData(nameof(userData.skillTicket), 0);
        }
        else
        {
            SaveManager.Instance.SetFieldData(nameof(userData.diamond), -oneCostDia * 10, true);
        }
        Gacha(10);
    }
    public void OnClickThirtyBtn()
    {
        if (userData.skillTicket >= oneCostTicket * 30) // 티켓 먼저 소모
        {
            SaveManager.Instance.SetFieldData(nameof(userData.skillTicket), -oneCostTicket * 30, true);
        }
        else if (userData.skillTicket >= oneCostTicket)
        {
            SaveManager.Instance.SetFieldData(nameof(userData.diamond), -(oneCostDia * (30 - userData.skillTicket / oneCostTicket)), true);
            SaveManager.Instance.SetFieldData(nameof(userData.skillTicket), 0);
        }
        else
        {
            SaveManager.Instance.SetFieldData(nameof(userData.diamond), -oneCostDia * 30, true);
        }
        Gacha(30);
    }
    #endregion

    #region Gacha
    private void Gacha(int count)
    {
        MyAtvData[] randSkill = new MyAtvData[count];
        var skillData = DataManager.Instance.activeData.data;
        var ownedSkills = userData.ownedSkills;
        int id;

        for (int i = 0; i < count; i++)
        {
            randSkill[i] = new MyAtvData();
            id = UnityEngine.Random.Range(0, skillData.Count);

            if (SkillManager.Instance.skillTab.allData[id].myAtvData.isOwned) // 이미 있는 스킬
            {
                SaveManager.Instance.UpdateSkillData(id, "evolvedCount", 
                    ownedSkills.Find((obj) => obj.id == id).evolvedCount + 1);
            }
            else // 새롭게 뽑은 스킬
            {
                randSkill[i].rcode = skillData[id].rcode;
                randSkill[i].id = skillData[id].id;
                randSkill[i].isOwned = true;
                ownedSkills.Add(randSkill[i]);
                SaveManager.Instance.SetFieldData(nameof(userData.ownedSkills), ownedSkills);
            }
            SkillManager.Instance.AddSkill(id);
            Debug.Log("스킬 id : " + id);
        }
    }
    #endregion
}
