using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Tutorial : UIBase
{
    [SerializeField] private GameObject TutorialMsg;

    private SaveManager saveManager => SaveManager.Instance;

    protected override void Awake()
    {
        base.Awake();
        if (saveManager.userData.isSetArrowOnEgg && (!saveManager.userData.isDoneTutorial))
        {
            TutorialMsg.SetActive(false);
            GuideManager.Instance.GuideArrow.SetActive(true);
            GuideManager.Instance.SetArrow(GuideManager.Instance.PxmToggle.gameObject);
        }           
    }

    public void HatchEgg()
    {
        if (!saveManager.userData.isSetArrowOnEgg)
        {
            saveManager.SetFieldData(nameof(saveManager.userData.isSetArrowOnEgg), true);
            GuideManager.Instance.SetArrow(GuideManager.Instance.PxmToggle.gameObject);
        }
    }

    public void TutorialDone()
    {
        GuideManager.Instance.GuideArrow.SetActive(false);
        saveManager.SetFieldData(nameof(saveManager.userData.isDoneTutorial), true);
        UIManager.Hide<Tutorial>();
    }
}
