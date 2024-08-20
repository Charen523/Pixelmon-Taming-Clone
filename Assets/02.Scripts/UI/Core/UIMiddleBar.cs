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
        EggLvPopup = await UIManager.Show<UIEggLvPopup>(this);
        AutoEggHatch = await UIManager.Show<UIAutoEggHatch>(EggHatch);
    }
    private void Start()
    {
        SetMiddleBarUI();
        UIManager.Instance.UpdateUI += UpdateMiddleBarUI;
        GuideManager.Instance.OnGuideAction += SetGuideArrow;
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
    }

    public void OnClickAutoBtn()
    {
        if (EggHatch.isAutoMode)
            EggHatch.isWantStopAuto = true;
        else
            AutoEggHatch.SetActive(true);            
    }

    private void SetGuideArrow(int guideIndex)
    {
        //GuideManager.Instance.GuideArrow.SetActive(true);

        switch (guideIndex)
        {
            case 6:
                //GuideManager.Instance.SetArrow(nestLvBtn);
                Debug.Log("둥지 버튼에 가이드ON");
                break;
        }
    }
}
