using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
    #region 등급 ui
    [SerializeField] private List<RateBg> rateBgs;
    [SerializeField] private List<PixelmonBg> pixelmonBgs;
    [SerializeField] private TextMeshProUGUI rateTxt;
    [SerializeField] private Image rateBg;
    [SerializeField] private Image pixelmonBg;
    [SerializeField] private Image pixelmonImg;
    #endregion
    #region 능력치 ui
    //[SerializeField] private TextMeshProUGUI atkRate;
    //[SerializeField] private TextMeshProUGUI atkTxt;
    //[SerializeField] private TextMeshProUGUI passiveRate;
    [SerializeField] private TextMeshProUGUI passiveTxt;
    //[SerializeField] private TextMeshProUGUI ownEffectRate;
    [SerializeField] private TextMeshProUGUI ownEffectTxt;
    #endregion
    [SerializeField] private TextMeshProUGUI pxmName;
    [SerializeField] private Button rePlaceBtn;

    private UIMiddleBar uiMiddleBar;
    private UserData userData;
    private SaveManager saveManager;
    private bool isOwnedPxm;

    private void OnEnable()
    {
        saveManager = SaveManager.Instance;
        userData = SaveManager.Instance.userData;
        uiMiddleBar = UIManager.Get<UIMiddleBar>();
        #region UI 셋팅        
        pxmName.text = uiMiddleBar.HatchPxmData.name;
        rateTxt.text = UIUtils.TranslateRank(uiMiddleBar.Rank);
        rateBg.sprite = PxmRankImgUtil.GetRankImage(uiMiddleBar.Rank, rateBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonBg.sprite = PxmRankImgUtil.GetRankImage(uiMiddleBar.Rank, pixelmonBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonImg.sprite = uiMiddleBar.HatchedPixelmonImg.sprite;

        //atkRate.text = uiMiddleBar.AbilityDic["Attack"].Item1;
        //atkTxt.text = uiMiddleBar.AbilityDic["Attack"].Item2.ToString();       
        #endregion        

        #region 이미 가지고 있는 픽셀몬인지 체크
        isOwnedPxm = false;
        foreach (var data in userData.ownedPxms)
        {
            if (uiMiddleBar.HatchPxmData.rcode == data.rcode)
            {
                isOwnedPxm = true; 
                break;
            }
        }

        if (isOwnedPxm) rePlaceBtn.gameObject.SetActive(true);
        else rePlaceBtn.gameObject.SetActive(false);
        #endregion
    }

    public void OnClickGetPixelmon(bool isReplaceBtn)
    {
        if (isReplaceBtn && isOwnedPxm == true) // 교체하기(교체 및 수집)
        {
            Debug.Log("ReplaceBtn");
        }
        else // 수집하기
        {
            Debug.Log("CollectBtn");
            if (!isOwnedPxm)
                GetFirst();
            else
                GetRepetition();
        }
        uiMiddleBar.OnClickGetPixelmon(isReplaceBtn);

        SetActive(false);
    }

    private void GetFirst()
    {
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "rcode", uiMiddleBar.HatchPxmData.rcode);
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "id", uiMiddleBar.HatchPxmData.id);
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "isOwned", true);
        PixelmonManager.Instance.UnLockedPixelmon(uiMiddleBar.HatchPxmData.id);
    }

    private void GetRepetition()
    {
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "evolvedCount", ++userData.ownedPxms[uiMiddleBar.HatchPxmData.id].evolvedCount);
        PixelmonManager.Instance.UnLockedPixelmon(uiMiddleBar.HatchPxmData.id);
    }
}
