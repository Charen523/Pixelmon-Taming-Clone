using DG.Tweening.Core.Easing;
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
    private List<UIBase> uiTabs = new List<UIBase>();
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private List<Toggle> toggles;

    int selectedIndex { get => toggles.FindIndex(obj  => obj.isOn); }

    private async void Awake()
    {
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
            if (GuideManager.Instance.Locks[index].activeInHierarchy)
            {
                string msg = "";
                switch (index)
                {
                    case 0:
                        msg = "해금조건: 첫 알뽑기";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 1:
                        msg = "해금조건: 첫 알뽑기";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 2:
                        msg = "해금조건: 쉬움 1 - 2 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 3:
                        msg = "해금조건: 쉬움 1 - 4 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 4:
                        msg = "해금조건: 쉬움 1 - 6 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    case 5:
                        msg = "해금조건: 쉬움 1 - 2 클리어";
                        UIManager.Instance.ShowWarn(msg);
                        return;
                    default:
                        break;
                }
            }
        }

        if (!SaveManager.Instance.userData.isDoneTutorial)
            GuideManager.Instance.SetArrow(GuideManager.Instance.PxmToggle.gameObject);

        overlayPanel.SetActive(selectedIndex >= 0);
        uiTabs.ForEach(obj => obj.gameObject.SetActive(false)); //모든 탭 끄기.
        if (selectedIndex >= 0)
        {
            uiTabs[selectedIndex].gameObject.SetActive(true);   
        }
    }
}
