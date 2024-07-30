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
    [SerializeField]
    protected Image slotIconBg;
    #endregion

    #region UI
    public int slotIndex;
    [SerializeField] protected TextMeshProUGUI lvTxt;
    [SerializeField] private Slider evolveSldr;
    [SerializeField] private TextMeshProUGUI evolveTxt;
    [SerializeField] protected GameObject[] stars;
    public GameObject equipIcon;
    public TextMeshProUGUI propertyEffectTxt;
    #endregion

    #region 데이터
    public PixelmonTab pxmtab;
    public PixelmonData pxmData;
    public MyPixelmonData myPxmData;
    public bool isOwned => myPxmData.isOwned;
    #endregion

    public virtual void InitSlot(PixelmonTab tab, PixelmonData data)
    {
        dataManager = DataManager.Instance;
        pxmtab = tab;
        pxmData = data;
        slotIcon.sprite = pxmData.icon;
        slotIconBg.sprite = pxmData.bgIcon;
        slotBtn.onClick.AddListener(OnClick);
        //if (isLocked) lockIcon.SetActive(false);
    }

    public virtual void UpdateSlot()
    {
        myPxmData = SaveManager.Instance.userData.ownedPxms[pxmData.id];
        slotIcon.color = Color.white;
        slotIconBg.color = Color.white;
        lvTxt.gameObject.SetActive(true);
        evolveSldr.gameObject.SetActive(true);

        SetPxmLv();
        SetStars();
        SetEvolveSldr();
    }

    public void SetPxmLv()
    {
        lvTxt.text = string.Format("Lv.{0}", myPxmData.lv);
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
        int maxNum = UIUtils.GetEvolveValue(myPxmData, pxmData);
        evolveSldr.maxValue = maxNum;
        evolveSldr.value = myPxmData.evolvedCount;
        evolveTxt.text = string.Format("{0}/{1}", myPxmData.evolvedCount, maxNum);
        if (myPxmData.evolvedCount >= maxNum)
        {
            pxmtab.userData.ownedPxms[myPxmData.id].isAdvancable = true;
            pxmtab.saveManager.UpdatePixelmonData(myPxmData.id, "isAdvancable", true);
        }
    }



    protected virtual void OnClick()
    {
        pxmtab.OnInfoPopUp(pxmData.id);
    }

    public void OnEvolved()
    {
        pxmtab.saveManager.UpdatePixelmonData(myPxmData.id, "isAdvancable", false);
        pxmtab.saveManager.UpdatePixelmonData(myPxmData.id, "evolvedCount", myPxmData.evolvedCount - UIUtils.GetEvolveValue(myPxmData, pxmData));
        pxmtab.saveManager.UpdatePixelmonData(myPxmData.id, "star", ++myPxmData.star);
        SetStars();
        SetEvolveSldr();
    }
}
