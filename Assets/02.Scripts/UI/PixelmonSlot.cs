using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PixelmonSlot : MonoBehaviour
{
    [SerializeField]
    protected RectTransform rectTr;
    [SerializeField]
    protected Button slotBtn;
    [SerializeField]
    protected Image slotIcon;

    public PixelmonData pixelmonData;
    public PixelmonTab pixelmontab;
    public TextMeshProUGUI propertyEffectTxt;
    public bool isPossessed => pixelmonData.isPossessed;

    public int slotIndex;
    [SerializeField]
    protected TextMeshProUGUI lvTxt;
    [SerializeField]
    private Slider evolveSldr;
    [SerializeField]
    private TextMeshProUGUI evolveTxt;
    [SerializeField]
    protected GameObject[] stars;

    private DataManager dataManager;
    // Start is called before the first frame update


    public virtual void InitSlot(PixelmonTab tab, PixelmonData data)
    {
        dataManager = DataManager.Instance;
        pixelmontab = tab;
        pixelmonData = data;
        slotIcon.sprite = pixelmonData.icon;
        slotBtn.onClick.AddListener(OnClick);
        UpdateSlot();
        //if (isLocked) lockIcon.SetActive(false);
    }

    public virtual void UpdateSlot()
    {
        slotIcon.sprite = pixelmonData.icon;
        lvTxt.text = string.Format("Lv.{0}", pixelmonData.lv);
        SetStars();
        SetEvolveSldr();
    }

    public void SetStars()
    {
        if (stars.Length > 0)
        {
            for (int i = 0; i < pixelmonData.star; i++)
            {
                stars[i].SetActive(true);
            }
        }
    }

    public void SetEvolveSldr()
    {
        int maxNum = GetEvolveValue();
        evolveSldr.maxValue = maxNum;
        evolveSldr.value = pixelmonData.currentCount;
        evolveTxt.text = string.Format("{0}/{1}", pixelmonData.currentCount, maxNum);
    }

    public int GetEvolveValue()
    {
        switch (pixelmonData.star)
        {
            case 0:
                return dataManager.GetData<EvolveData>(pixelmonData.rank).start1;
            case 1:
                return dataManager.GetData<EvolveData>(pixelmonData.rank).start2;
            case 2:
                return dataManager.GetData<EvolveData>(pixelmonData.rank).start3;
            case 3:
                return dataManager.GetData<EvolveData>(pixelmonData.rank).start4;
            case 4:
                return dataManager.GetData<EvolveData>(pixelmonData.rank).start5;
            default:
                return 0;
        }
    }

    protected virtual void OnClick()
    {
        if (pixelmontab.tabState == TabState.Normal)
        {
            pixelmontab.OnClickSlot(pixelmonData.id, rectTr);
        }
    }
}
