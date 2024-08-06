using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillGachaBtn : MonoBehaviour
{
    public Button Btn;
    public GameObject TicketIcon;
    public GameObject DiaIcon;
    public TextMeshProUGUI TicketCostTxt;
    public TextMeshProUGUI DiaCostTxt;

    public void HideTicket()
    {
        TicketIcon.SetActive(false);
        TicketCostTxt.gameObject.SetActive(false);
    }

    public void HideDia()
    {
        DiaIcon.SetActive(false);
        DiaCostTxt.gameObject.SetActive(false);
    }

    public void SetTicket(int cost)
    {
        TicketIcon.SetActive(true);
        TicketCostTxt.gameObject.SetActive(true);
        TicketCostTxt.text = cost.ToString();
    }

    public void SetDia(int cost)
    {
        DiaIcon.SetActive(true);
        DiaCostTxt.gameObject.SetActive(true);
        DiaCostTxt.text = cost.ToString();
    }

    public void SetAllCost(int ticketCost, int diaCost)
    {
        SetTicket(ticketCost);
        SetDia(diaCost);
    }
}
