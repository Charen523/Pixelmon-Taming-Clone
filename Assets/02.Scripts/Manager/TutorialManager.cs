using System;
using UnityEngine;

public enum OpenTabType
{
    Skill,
    Farm,
    Dungeon
}
public class TutorialManager : Singleton<TutorialManager>
{
    public event Action<OpenTabType> TutorialEvent;

    public GameObject ShopLock;
    public GameObject SkillLock;
    public GameObject FarmLock;
    public GameObject DungeonLock;
    public int _themeNum => StageManager.Instance.themeNum;
    public int ThemeNum
    {
        get => _themeNum;
        set
        {
            if (_themeNum != value)
            {
                OpenTab(_themeNum);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        var userData = SaveManager.Instance.userData;

        if (userData.isOpenSkillTab)
        {
            ShopLock.SetActive(false);
            SkillLock.SetActive(false);
        }
        
        if(userData.isOpenFarmTab)
            FarmLock.SetActive(false);

        if (userData.isOpenDungeonTab)
            DungeonLock.SetActive(false);
    }

    private void OpenTab(int num)
    {
        switch (num)
        {
            case 1:
                ShopLock.SetActive(false);
                SkillLock.SetActive(false);
                break;
            case 2:
                FarmLock.SetActive(false);
                break;
            case 3:
                DungeonLock.SetActive(false);
                break;
        }
    }
}
