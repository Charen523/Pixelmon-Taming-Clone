using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum eTabs
{
    UIPixelmonTab,
    UIUpgradeTab,
    UISkilltab,
    UIFarmTab,
    UIDungeonTab,
    UIShopTab
}

public class UIBottomBar : MonoBehaviour
{
    GuideManager guideManager;

    private List<UIBase> uiTabs = new List<UIBase>();
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private List<Toggle> toggles;

    private bool isGuideOn = false;
    private int guidingToggle = -1;

    int selectedIndex { get => toggles.FindIndex(obj  => obj.isOn); }

    private async void Awake()
    {
        guideManager = GuideManager.Instance;
        guideManager.OnGuideAction += SetGuideArrow;

        var names = Enum.GetNames(typeof(eTabs));

        for (int i = 0; i < names.Length; i++)
        {
            uiTabs.Add(await UIManager.Show(names[i]));
        }
    }

    public void OnvalueChanged()
    {
        GameObject nextObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        
        if (int.TryParse(nextObject.name, out int index))
        {
            if (guideManager.Locks[index].activeInHierarchy)
            {
                string msg = "";
                switch (index)
                {
                    case 0:
                        return;
                    case 1:
                        msg = "해금조건: 퀘스트8 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 2:
                        msg = "해금조건: 퀘스트20 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 3:
                        msg = "해금조건: 퀘스트34 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 4:
                        msg = "해금조건: 퀘스트50 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 5:
                        msg = "해금조건: 퀘스트20 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    default:
                        break;
                }
            }
        }
        
        overlayPanel.SetActive(selectedIndex >= 0);
        uiTabs.ForEach(obj => obj.gameObject.SetActive(false)); //모든 탭 끄기.
        if (selectedIndex >= 0)
        {
            uiTabs[selectedIndex].gameObject.SetActive(true);   
            if (isGuideOn && guidingToggle == selectedIndex)
            {
                isGuideOn = false;
                guidingToggle = -1;

                switch(selectedIndex)
                {
                    case 0:
                        UIPixelmonTab tab = uiTabs[selectedIndex].GetComponent<UIPixelmonTab>();
                        tab.InvokePixelmonTabGuide();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void SetGuideArrow(int guideIndex)
    {
        isGuideOn = true;

        switch (guideIndex)
        {
            case 1:
                guideManager.SetArrow(toggles[0].gameObject);
                guidingToggle = 0;
                guideManager.GuideArrow.SetActive(true);
                break;
            case 3: //일괄편성
                guideManager.SetArrow(toggles[0].gameObject);
                guidingToggle = 0;
                guideManager.GuideArrow.SetActive(true);
                break;
            case 9: //공격력 업그레이드
                guideManager.SetArrow(toggles[1].gameObject);
                guidingToggle = 1;
                guideManager.GuideArrow.SetActive(true);
                break;
            case 10: //먹이주기
                guideManager.SetArrow(toggles[0].gameObject);
                guidingToggle = 0;
                guideManager.GuideArrow.SetActive(true);
                break;
            case 21: //스킬 뽑기
                guideManager.SetArrow(toggles[5].gameObject);
                guidingToggle = 5;
                guideManager.GuideArrow.SetActive(true);
                break;
            case 23: //스킬 장착
                guideManager.SetArrow(toggles[2].gameObject);
                guidingToggle = 2;
                guideManager.GuideArrow.SetActive(true);
                break;
            case 35: //씨앗심기
                guideManager.SetArrow(toggles[3].gameObject);
                guidingToggle = 3;
                guideManager.GuideArrow.SetActive(true);
                break;
            case 45: //수확하기
                guideManager.SetArrow(toggles[3].gameObject);
                guideManager.GuideArrow.SetActive(true);
                guidingToggle = 3;
                break;
            case 51: //골드 던전
                guideManager.SetArrow(toggles[4].gameObject);
                guidingToggle = 4;
                guideManager.GuideArrow.SetActive(true);
                break;
        }
    }
}

