using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
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

    private UIEggLvPopup EggLvPopup;
    private UserData userData => SaveManager.Instance.userData;

    protected override async void Awake()
    {
        EggLvPopup = await UIManager.Show<UIEggLvPopup>(this);
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
    }

    public void OnClickAutoBtn()
    {
    }
}
