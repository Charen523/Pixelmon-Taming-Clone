using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonSlot : MonoBehaviour
{
    private DataManager dataManager;

    #region 슬롯정보
    [SerializeField]
    protected RectTransform rectTr;
    [SerializeField]
    protected Button slotBtn;
    [SerializeField]
    protected Image slotIcon;
    #endregion

    #region UI
    public int slotIndex;
    [SerializeField]
    protected TextMeshProUGUI lvTxt;
    [SerializeField]
    private Slider evolveSldr;
    [SerializeField]
    private TextMeshProUGUI evolveTxt;
    public TextMeshProUGUI propertyEffectTxta;
    [SerializeField]
    protected GameObject[] stars;
    #endregion

    #region 데이터
    public PixelmonTab pxmtab;
    public PixelmonData pxmData;
    public MyPixelmonData myPxmData;
    public bool isPossessed => myPxmData.isPossessed;
    #endregion

    public virtual void InitSlot(PixelmonTab tab, PixelmonData data)
    {
        dataManager = DataManager.Instance;
        pxmtab = tab;
        pxmData = data;
        slotIcon.sprite = pxmData.icon;
        slotBtn.onClick.AddListener(OnClick);
        UpdateSlot();
        //if (isLocked) lockIcon.SetActive(false);
    }

    public virtual void UpdateSlot()
    {
        slotIcon.sprite = pxmData.icon;
        lvTxt.text = string.Format("Lv.{0}", myPxmData.lv);
        SetStars();
        SetEvolveSldr();
    }

    public void SetStars()
    {
        if (stars.Length > 0)
        {
            for (int i = 0; i < myPxmData.star; i++)
            {
                stars[i].SetActive(true);
            }
        }
    }

    public void SetEvolveSldr()
    {
        int maxNum = GetEvolveValue();
        evolveSldr.maxValue = maxNum;
        evolveSldr.value = myPxmData.evolvedCount;
        evolveTxt.text = string.Format("{0}/{1}", myPxmData.evolvedCount, maxNum);
    }

    public int GetEvolveValue()
    {
        switch (myPxmData.star)
        {
            case 0:
                return dataManager.GetData<EvolveData>(pxmData.rank).start1;
            case 1:
                return dataManager.GetData<EvolveData>(pxmData.rank).start2;
            case 2:
                return dataManager.GetData<EvolveData>(pxmData.rank).start3;
            case 3:
                return dataManager.GetData<EvolveData>(pxmData.rank).start4;
            case 4:
                return dataManager.GetData<EvolveData>(pxmData.rank).start5;
            default:
                return 0;
        }
    }

    protected virtual void OnClick()
    {
        if (pxmtab.tabState == TabState.Normal)
        {
            pxmtab.OnClickSlot(pxmData.id, rectTr);
        }
    }
}
