using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlot : MonoBehaviour
{
    SkillTab skillTab;

    #region 슬롯정보
    [SerializeField] protected int slotIndex;
    [SerializeField]
    protected RectTransform rectTr;
    [SerializeField]
    protected Button slotBtn;
    [SerializeField]
    protected Image slotIcon;
    [SerializeField]
    protected Image slotIconBg;

    #endregion

    #region UI
    public TextMeshProUGUI skillRankTxt;
    [SerializeField] private GameObject equipIcon;
    [SerializeField] protected TextMeshProUGUI skillLv;
    [SerializeField] private Slider evolvedSldr;
    [SerializeField] private TextMeshProUGUI evolvedCount;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void InitSkillSlot(SkillTab tab, int index)
    {
        skillTab = tab;
        slotIndex = index;
    }

    public void SetRankColor()
    {

    }


}
