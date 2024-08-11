using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMsg : UIBase
{
    public void OnClick()
    {
        SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.isDoneTutorialMsg), true);
        UIManager.Hide<TutorialMsg>();
    }
}
