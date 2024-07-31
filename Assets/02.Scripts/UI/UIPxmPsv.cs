using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPxmPsv : MonoBehaviour
{
    public TextMeshProUGUI OldPsvRankTxt;
    public TextMeshProUGUI PsvNameTxt;
    public TextMeshProUGUI OldPsvValueTxt;
    public Image ArrowImg;
    public TextMeshProUGUI NewPsvRankTxt;
    public TextMeshProUGUI NewPsvValueTxt;

    public AbilityType PsvType;
    public string PsvName;
    public string NewPsvRank;
    public float NewPsvValue;
}
