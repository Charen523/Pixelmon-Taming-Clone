using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIHatchWarnPopup : UIBase
{
    private EggHatch eggHatch;
    public void SetPopup(EggHatch eggHatch)
    {
        this.eggHatch = eggHatch;
    }

    public void OnClickNoBtn()
    {
        gameObject.SetActive(false);
    }

    public void OnClickYesBtn()
    {
        eggHatch.GetPixelmon(true);
        gameObject.SetActive(false);
    }
}
