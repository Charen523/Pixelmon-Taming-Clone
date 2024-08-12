using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum eTabs
{
    UIUpgradeTab,
    UIPixelmonTab,
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

    public void OnvalueChanged(int index)
    {
        if (TutorialManager.Instance.Locks[index].activeInHierarchy)
        {
            string msg = "";
            switch(index)
            {
                case 0:
                    msg = "해금조건: 첫 알뽑기";
                    break;
                case 1:
                    msg = "해금조건: 첫 알뽑기";
                    break;
                case 2:
                    msg = "해금조건: 쉬움 1 - 5 클리어";
                    break;
                case 3:
                    msg = "해금조건: 쉬움 1 - 10 클리어";
                    break;
                case 4:
                    msg = "해금조건: 쉬움 1 - 15 클리어";
                    break;
                case 5:
                    msg = "해금조건: 쉬움 1 - 5 클리어";
                    break;
            }
            UIManager.Instance.ShowWarn(msg);
            return;
        }

        overlayPanel.SetActive(selectedIndex >= 0);
        uiTabs.ForEach(obj => obj.gameObject.SetActive(false)); //모든 탭 끄기.
        if (selectedIndex >= 0)
        {
            uiTabs[selectedIndex].gameObject.SetActive(true);   
        }
    }
}
