using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EggImg : BaseBg
{
}
public class UIMiddleBar : UIBase
{    
    public TextMeshProUGUI EggLvText;
    public TextMeshProUGUI EggCntText;
    public GameObject nestLvBtn;

    public EggHatch EggHatch;

    private UIEggLvPopup EggLvPopup;
    private UIAutoEggHatch AutoEggHatch;
    private UserData userData => SaveManager.Instance.userData;

    protected override async void Awake()
    {
        GuideManager.Instance.OnGuideAction += SetGuideArrow;
        EggLvPopup = await UIManager.Show<UIEggLvPopup>(this);
        AutoEggHatch = await UIManager.Show<UIAutoEggHatch>(EggHatch);
    }
    private void Start()
    {
        SetMiddleBarUI();
        UIManager.Instance.UpdateUI += UpdateMiddleBarUI;
    }

    private void SetMiddleBarUI()
    {
        EggCntText.text = userData.eggCount.ToString();
        EggLvText.text = userData.eggLv.ToString();
    }

    public void UpdateMiddleBarUI(DirtyUI dirtyUI)
    {
        switch (dirtyUI)
        {
            case DirtyUI.EggCount:
                EggCntText.text = userData.eggCount.ToString();
                break;
            case DirtyUI.EggLv:
                EggLvText.text = userData.eggLv.ToString();
                break;
        }   
    }

    public void OnClickEggLvBtn()
    {
        EggLvPopup.SetActive(true);
        EggLvPopup.SetPopup(this);
        EggLvPopup.EggLvGuide();
    }

    public void OnClickAutoBtn()
    {
        if (EggHatch.isAutoMode)
            EggHatch.isWantStopAuto = true;
        else
            AutoEggHatch.SetActive(true);            
    }

    public void SetGuideArrow(int guideIndex)
    {
        switch (guideIndex)
        {
            case 6:
                if (userData.eggLv < 2)
                {
                    GuideManager.Instance.GuideArrow.SetActive(true);
                    GuideManager.Instance.SetArrow(nestLvBtn, 20f);
                }
                break;
        }
    }
}
