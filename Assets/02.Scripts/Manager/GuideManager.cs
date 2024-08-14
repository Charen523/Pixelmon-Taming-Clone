using System;
using UnityEngine;

public enum OpenTabType
{
    Skill,
    Farm,
    Dungeon
}
public class GuideManager : Singleton<GuideManager>
{
    public GameObject[] Locks;

    public GameObject GuideArrow;

    protected override void Awake()
    {
        base.Awake();
        var userData = SaveManager.Instance.userData;

        if (userData.isEggHatched)
        {
            Locks[0].SetActive(false);
            Locks[1].SetActive(false);
        }

        if (userData.isOpenSkillTab)
        {
            Locks[2].SetActive(false);
            Locks[5].SetActive(false);
        }
        
        if(userData.isOpenFarmTab)
            Locks[3].SetActive(false);

        if (userData.isOpenDungeonTab)
            Locks[4].SetActive(false);

        if (SaveManager.Instance.userData.isSetArrowOnEgg)
            GuideArrow.SetActive(false);

            StageManager.Instance.OnOpenTab += OpenTab;
    }

    private void OpenTab(int num)
    {
        var userData = SaveManager.Instance.userData;

        switch (num)
        {
            case 3:
                if (!userData.isOpenSkillTab)
                {
                    SaveManager.Instance.SetFieldData(nameof(userData.isOpenSkillTab), true);
                    Locks[2].SetActive(false);
                    Locks[5].SetActive(false);
                }
                break;
            case 5:
                if (!userData.isOpenFarmTab)
                {
                    SaveManager.Instance.SetFieldData(nameof(userData.isOpenFarmTab), true);
                    Locks[3].SetActive(false);
                }                    
                break;
            case 7:
                if (!userData.isOpenDungeonTab)
                {
                    SaveManager.Instance.SetFieldData(nameof(userData.isOpenDungeonTab), true);
                    Locks[4].SetActive(false);
                }                 
                break;
        }
    }

    // obj의 피벗 y를 1(center-top)로 맞추시오. 더 위로 올리고 싶으면 addPos로 값 추가
    public void SetArrow(GameObject obj, float addYPos = 0)
    {
        Vector3 currentPosition = obj.transform.position;
        Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y + 20f + addYPos, currentPosition.z);
        GuideArrow.transform.position = newPosition;
    }

    public void HideArrow()
    {
        if (!SaveManager.Instance.userData.isSetArrowOnEgg)
        {
            GuideArrow.SetActive(false);
            SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.isSetArrowOnEgg), true);
        }           
    }
}
