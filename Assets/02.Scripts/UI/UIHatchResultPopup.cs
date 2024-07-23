using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public class RateBg : BaseBg
{
}

[System.Serializable]
public class PixelmonBg : BaseBg
{
}

public class UIHatchResultPopup : UIBase
{
    [SerializeField] private List<RateBg> rateBgs;
    [SerializeField] private List<PixelmonBg> pixelmonBgs;
    [SerializeField] private TextMeshProUGUI rateTxt;
    [SerializeField] private Image rateBg;
    [SerializeField] private Image pixelmonBg;
    [SerializeField] private Image pixelmonImg;
    [SerializeField] private Button rePlaceBtn;
    private UIMiddleBar uiMiddleBar;

    private void OnEnable()
    {
        #region UI 셋팅
        uiMiddleBar = UIManager.Get<UIMiddleBar>();
        rateBg.sprite = PxmRankImgUtil.GetRankImage(uiMiddleBar.rank, rateBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonBg.sprite = PxmRankImgUtil.GetRankImage(uiMiddleBar.rank, pixelmonBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonImg.sprite = uiMiddleBar.HatchedPixelmonImg.sprite;
        #endregion
    }

    public void OnClickGetPixelmon(bool isReplace)
    {
        var userData = SaveManager.Instance.userData;

        #region 이미 가지고 있는 픽셀몬인지 체크
        bool isOwnedPxm = false;
        foreach (var data in userData.ownedPxms)
        {
            if(uiMiddleBar.HatchPxmData.rcode == data.rcode)
            {
                isOwnedPxm = true; break;
            }
        }

        if(isOwnedPxm) rePlaceBtn.gameObject.SetActive(true);
        else rePlaceBtn.gameObject.SetActive(false);
        #endregion

        if (isReplace && isOwnedPxm == true) // 교체하기(교체 및 수집)
        {
            Debug.Log("ReplaceBtn");
        }
        else // 수집하기
        {
            Debug.Log("CollectBtn");
        }

        uiMiddleBar.OnClickGetPixelmon(isReplace);

        SetActive(false);
    }
}
