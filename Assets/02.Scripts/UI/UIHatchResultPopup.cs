using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RateBg
{
    public PixelmonRank rank;
    public Sprite img;
    public List<Dictionary<PixelmonRank, Sprite>> bgs;
}

public class UIHatchResultPopup : UIBase
{
    [SerializeField] private List<RateBg> RateBgs;
    [SerializeField] private Image pixelmonBg;
    [SerializeField] private Image pixelmonImg;
    [SerializeField] private Button rePlaceBtn;
    private UIMiddleBar uiMiddleBar;

    private void OnEnable()
    {
        #region UI 셋팅
        uiMiddleBar = UIManager.Get<UIMiddleBar>();
        pixelmonBg.sprite = GetCommonRankImage(uiMiddleBar.rank);
        pixelmonImg.sprite = uiMiddleBar.HatchedPixelmonImg.sprite;
        #endregion
    }

    public Sprite GetCommonRankImage(PixelmonRank rank)
    {
        foreach (RateBg rateBg in RateBgs)
        {
            if (rateBg.rank == rank)
            {
                return rateBg.img;
            }
        }
        return null; // 해당 rank가 없을 경우 null 반환
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
