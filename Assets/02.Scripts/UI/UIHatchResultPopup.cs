using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHatchResultPopup : UIBase
{
    public void OnClickGetPixelmon(bool isReplace)
    {
        UIManager.Get<UIMiddleBar>().OnClickGetPixelmon(isReplace);

        SetActive(false);
    }
}
