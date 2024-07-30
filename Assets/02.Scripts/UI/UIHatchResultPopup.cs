using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

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
    #region 등급 UI
    [Header("등급 UI")]
    [SerializeField] private List<RateBg> rateBgs;
    [SerializeField] private List<PixelmonBg> pixelmonBgs;
    [SerializeField] private TextMeshProUGUI pxmName;
    [SerializeField] private TextMeshProUGUI rateTxt;
    [SerializeField] private Image rateBg;
    [SerializeField] private Image pixelmonBg;
    [SerializeField] private Image pixelmonImg;
    #endregion
    #region 능력치 UI
    [Header("능력치 UI")]
    [SerializeField] private TextMeshProUGUI NewAtkValueTxt;
    [SerializeField] private TextMeshProUGUI TraitTypeTxt;
    [SerializeField] private TextMeshProUGUI NewTraitValueTxt;

    [SerializeField] private UIPxmPsv[] Psv = new UIPxmPsv[4];

    [SerializeField] private TextMeshProUGUI OwnHpValueTxt;
    [SerializeField] private TextMeshProUGUI OwnDefenseValueTxt;
    #endregion
    [SerializeField] private Button rePlaceBtn;

    private UIMiddleBar uiMiddleBar;
    private UserData userData => SaveManager.Instance.userData;
    private SaveManager saveManager => SaveManager.Instance;
    private MyPixelmonData hatchedPxmData;
    private bool isOwnedPxm;

    private void OnEnable()
    {
        uiMiddleBar = UIManager.Get<UIMiddleBar>();

        #region 소환된 픽셀몬 정보 UI        
        pxmName.text = uiMiddleBar.HatchPxmData.name;
        rateTxt.text = UIUtils.TranslateRank(uiMiddleBar.Rank);
        rateBg.sprite = PxmRankImgUtil.GetRankImage(uiMiddleBar.Rank, rateBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonBg.sprite = PxmRankImgUtil.GetRankImage(uiMiddleBar.Rank, pixelmonBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonImg.sprite = uiMiddleBar.HatchedPixelmonImg.sprite;
        #endregion        

        #region 이미 가지고 있는 픽셀몬인지 체크 & 픽셀몬 능력치 UI
        isOwnedPxm = false;
        foreach (var data in userData.ownedPxms)
        {
            if (uiMiddleBar.HatchPxmData.rcode == data.rcode)
            {
                isOwnedPxm = true;
                hatchedPxmData = data;
                break;
            }
        }

        if (isOwnedPxm)
        {
            rePlaceBtn.gameObject.SetActive(true);
            OwnedPxmUI();
        }
        else
        {
            rePlaceBtn.gameObject.SetActive(false);
            SetFirstPsvValue();
            FirstPxmUI();
        }

        NewAtkValueTxt.text = uiMiddleBar.HatchPxmData.basePerAtk.ToString();
        TraitTypeTxt.text = uiMiddleBar.HatchPxmData.trait.TranslateTraitString();
        NewTraitValueTxt.text = uiMiddleBar.HatchPxmData.traitValue.ToString();
        OwnHpValueTxt.text = uiMiddleBar.HatchPxmData.basePerHp.ToString();
        OwnDefenseValueTxt.text = uiMiddleBar.HatchPxmData.basePerDef.ToString();
        #endregion
    }
    private void OwnedPxmUI()
    {
        Psv[0].OldPsvRankTxt.gameObject.SetActive(true);
        Psv[0].OldPsvValueTxt.gameObject.SetActive(true);
        Psv[0].ArrowImg.gameObject.SetActive(true);
        for (int i = 1; i <= 3; i++)
        {
            Psv[i].gameObject.SetActive(i * 2 - 1 <= hatchedPxmData.star);
        }
    }

    private void SetFirstPsvValue()
    {
        var basePsvData = RandAbilityUtil.RandAilityData();
        var randAbility = RandAbilityUtil.PerformAbilityGacha((AbilityType)basePsvData.psvEnum, basePsvData.maxRate);
        Psv[0].NewPsvRank = randAbility.AbilityRank;
        Psv[0].NewPsvValue = randAbility.AbilityValue;
    }
    private void FirstPxmUI()
    {
        Psv[0].NewPsvRankTxt.text = Psv[0].NewPsvRank.ToString();
        Psv[0].NewPsvValueTxt.text = Psv[0].NewPsvValue.ToString();

        Psv[0].OldPsvRankTxt.gameObject.SetActive(false);
        Psv[0].OldPsvValueTxt.gameObject.SetActive(false);
        Psv[0].ArrowImg.gameObject.SetActive(false);
        for (int i = 1; i <= 3; i++)
        {
            Psv[i].gameObject.SetActive(false);
        }
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
