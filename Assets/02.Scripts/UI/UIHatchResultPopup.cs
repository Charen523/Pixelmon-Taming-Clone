using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHatchResultPopup : UIBase
{
    public Dictionary<PixelmonRank, Sprite> RateDic = new Dictionary<PixelmonRank, Sprite>();
    [SerializeField] private Image pixelmonBg;
    [SerializeField] private Image pixelmonImg;
    public void OnClickGetPixelmon(bool isReplace)
    {
        pixelmonBg.sprite = RateDic[UIManager.Get<UIMiddleBar>().rank];
        pixelmonImg.sprite = UIManager.Get<UIMiddleBar>().HatchedPixelmonImg.sprite;
        
        UIManager.Get<UIMiddleBar>().OnClickGetPixelmon(isReplace);

        SetActive(false);
    }
}
