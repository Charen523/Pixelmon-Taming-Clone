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
    public GameObject[] Locks;

    public GameObject TutorialArrow;

    protected override void Awake()
    {
        base.Awake();
        var userData = SaveManager.Instance.userData;

        if (userData.isOpenSkillTab)
        {
            Locks[5].SetActive(false);
            Locks[2].SetActive(false);
        }
        
        if(userData.isOpenFarmTab)
            Locks[3].SetActive(false);

        if (userData.isOpenDungeonTab)
            Locks[4].SetActive(false);

        if (SaveManager.Instance.userData.isSetArrowOnEgg)
            TutorialArrow.SetActive(false);

            StageManager.Instance.OnChangeThemeNum += OpenTab;
    }

    private void OpenTab(int num)
    {
        var userData = SaveManager.Instance.userData;

        switch (num)
        {
            case 2:
                if (!userData.isOpenSkillTab)
                {
                    SaveManager.Instance.SetFieldData(nameof(userData.isOpenSkillTab), true);
                    Locks[5].SetActive(false);
                    Locks[2].SetActive(false);
                }
                break;
            case 3:
                if (!userData.isOpenFarmTab)
                {
                    SaveManager.Instance.SetFieldData(nameof(userData.isOpenFarmTab), true);
                    Locks[3].SetActive(false);
                }                    
                break;
            case 1:
                if (!userData.isOpenDungeonTab)
                {
                    SaveManager.Instance.SetFieldData(nameof(userData.isOpenDungeonTab), true);
                    Locks[4].SetActive(false);
                }                 
                break;
        }
    }

    public void SetArrow(GameObject obj, float addYPos)
    {
        Vector3 currentPosition = obj.transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + addYPos, currentPosition.z);
        TutorialArrow.transform.position = newPosition;
    }

    public void HideArrow()
    {
        if (!SaveManager.Instance.userData.isSetArrowOnEgg)
        {
            TutorialArrow.SetActive(false);
            SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.isSetArrowOnEgg), true);
        }           
    }
}
