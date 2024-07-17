using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum eTabs
{
    UIStatusupTab,
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

    public void OnvalueChanged()
    {
        overlayPanel.SetActive(selectedIndex >= 0);
        uiTabs.ForEach(obj => obj.gameObject.SetActive(false)); //모든 탭 끄기.
        if (selectedIndex >= 0)
        {
            uiTabs[selectedIndex].gameObject.SetActive(true);   
        }
    }
}
