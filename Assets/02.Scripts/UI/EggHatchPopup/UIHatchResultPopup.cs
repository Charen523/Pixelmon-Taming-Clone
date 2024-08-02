using System;
using System.Collections.Generic;
using System.Text;
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
    [SerializeField] private TextMeshProUGUI AtkValueTxt;
    [SerializeField] private TextMeshProUGUI TraitTypeTxt;
    [SerializeField] private TextMeshProUGUI TraitValueTxt;

    [SerializeField] private UIPxmPsv[] UIPsv = new UIPxmPsv[4];

    [SerializeField] private TextMeshProUGUI OwnHpValueTxt;
    [SerializeField] private TextMeshProUGUI OwnDefenseValueTxt;
    #endregion
    [SerializeField] private Button rePlaceBtn;

    private UIMiddleBar uiMiddleBar;
    private UserData userData => SaveManager.Instance.userData;
    
    public void SetPopup(UIMiddleBar middleBar)
    {
        SaveManager.Instance.SetData(nameof(userData.isGetPxm), false);
        uiMiddleBar = middleBar;
        #region 소환된 픽셀몬 정보 UI        
        pxmName.text = uiMiddleBar.HatchPxmData.name;
        rateTxt.text = UIUtils.TranslateRank(uiMiddleBar.Rank);
        rateBg.sprite = PxmRankImgUtil.GetRankImage(uiMiddleBar.Rank, rateBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonBg.sprite = PxmRankImgUtil.GetRankImage(uiMiddleBar.Rank, pixelmonBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonImg.sprite = uiMiddleBar.HatchedPixelmonImg.sprite;
        #endregion

        #region 이미 가지고 있는 픽셀몬인지 체크 & 픽셀몬 능력치 UI 셋팅
        AtkValueTxt.text = uiMiddleBar.HatchPxmData.basePerAtk.ToString("F2");
        TraitTypeTxt.text = uiMiddleBar.HatchPxmData.trait.TranslateTraitString();
        TraitValueTxt.text = uiMiddleBar.HatchPxmData.traitValue.ToString("F2");
        OwnHpValueTxt.text = uiMiddleBar.HatchPxmData.basePerHp.ToString("F2");
        OwnDefenseValueTxt.text = uiMiddleBar.HatchPxmData.basePerDef.ToString("F2");

        if (uiMiddleBar.IsOwnedPxm)
        {
            rePlaceBtn.gameObject.SetActive(true);
            OwnedPxmUI();
        }
        else
        {
            rePlaceBtn.gameObject.SetActive(false);
            FirstPxmUI();
        }      
        #endregion
    }

    private void OwnedPxmUI()
    {
        UIPsv[0].NewPsvRankTxt.gameObject.SetActive(true);
        UIPsv[0].OldPsvValueTxt.gameObject.SetActive(true);
        UIPsv[0].ArrowImg.gameObject.SetActive(true);
        for (int i = 0; i < uiMiddleBar.HatchMyPxmData.psvSkill.Count; i++)
        {
            UIPsv[i].gameObject.SetActive(true);

            UIPsv[i].OldPsvRankTxt.text = uiMiddleBar.HatchMyPxmData.psvSkill[i].psvRank;
            UIPsv[i].PsvNameTxt.text = uiMiddleBar.HatchMyPxmData.psvSkill[i].psvName;
            UIPsv[i].OldPsvValueTxt.text = new StringBuilder().Append(uiMiddleBar.HatchMyPxmData.psvSkill[i].psvValue.ToString("F2")).Append('%').ToString();

            UIPsv[i].NewPsvRankTxt.text = uiMiddleBar.PsvData[i].NewPsvRank.ToString();
            UIPsv[i].NewPsvValueTxt.text = new StringBuilder().Append(uiMiddleBar.PsvData[i].NewPsvValue.ToString("F2")).Append('%').ToString();
            if (uiMiddleBar.PsvData[i].NewPsvValue > uiMiddleBar.HatchMyPxmData.psvSkill[i].psvValue)
                UIPsv[i].NewPsvValueTxt.HexColor("#78FF1E");
            else if(uiMiddleBar.PsvData[i].NewPsvValue < uiMiddleBar.HatchMyPxmData.psvSkill[i].psvValue)
                UIPsv[i].NewPsvValueTxt.HexColor("#FF0A0A");
        }
        for (int i = 3; i >= uiMiddleBar.HatchMyPxmData.psvSkill.Count; i--)
        {
            UIPsv[i].gameObject.SetActive(false);
        }
    }

    private void FirstPxmUI()
    {
        UIPsv[0].PsvNameTxt.text = uiMiddleBar.PsvData[0].PsvName;
        UIPsv[0].OldPsvRankTxt.text = uiMiddleBar.PsvData[0].NewPsvRank.ToString();
        UIPsv[0].NewPsvValueTxt.text = new StringBuilder().Append(uiMiddleBar.PsvData[0].NewPsvValue.ToString("F2")).Append('%').ToString();
        UIPsv[0].NewPsvValueTxt.HexColor("#78FF1E");

        UIPsv[0].NewPsvRankTxt.gameObject.SetActive(false);
        UIPsv[0].OldPsvValueTxt.gameObject.SetActive(false);
        UIPsv[0].ArrowImg.gameObject.SetActive(false);
        for (int i = 1; i <= 3; i++)
        {
            UIPsv[i].gameObject.SetActive(false);
        }
    }

    public void OnClickGetPixelmon(bool isReplaceBtn)
    {
        if (isReplaceBtn && uiMiddleBar.IsOwnedPxm == true) // 교체하기(교체 및 수집)
        {
            ReplacePsv();
        }
        else // 수집하기
        {
            if (!uiMiddleBar.IsOwnedPxm)
                GetFirst();
            else
                GetRepetition();
        }
        uiMiddleBar.GetPixelmon();

        SetActive(false);
    }

    private void GetFirst()
    {
        #region Init Value
        List<PsvSkill> firstPsv = new List<PsvSkill>();
        firstPsv.Add(new PsvSkill
        {
            psvType = uiMiddleBar.PsvData[0].PsvType,
            psvName = uiMiddleBar.PsvData[0].PsvName,
            psvRank = uiMiddleBar.PsvData[0].NewPsvRank,
            psvValue = uiMiddleBar.PsvData[0].NewPsvValue
        });

        float[] ownEffectValue = { uiMiddleBar.HatchPxmData.basePerHp, uiMiddleBar.HatchPxmData.basePerDef };
        #endregion

        #region Update Value
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "rcode", uiMiddleBar.HatchPxmData.rcode);
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "id", uiMiddleBar.HatchPxmData.id);
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "isOwned", true);
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "atkValue", uiMiddleBar.HatchPxmData.basePerAtk);
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "traitType", uiMiddleBar.HatchPxmData.trait.TranslateTraitString());
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "traitValue", uiMiddleBar.HatchPxmData.traitValue);
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "psvSkill", firstPsv);
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "ownEffectValue", ownEffectValue);
        PixelmonManager.Instance.UnLockedPixelmon(uiMiddleBar.HatchPxmData.id);
        #endregion
    }

    private void GetRepetition()
    {
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "evolvedCount", ++userData.ownedPxms[uiMiddleBar.HatchPxmData.id].evolvedCount);
        PixelmonManager.Instance.UnLockedPixelmon(uiMiddleBar.HatchPxmData.id);
    }

    private void ReplacePsv()
    {
        List<PsvSkill> newPsvs = new List<PsvSkill>();
        for (int i = 0; i < uiMiddleBar.HatchMyPxmData.psvSkill.Count; i++)
        {
            newPsvs.Add(new PsvSkill
            {
                psvType = uiMiddleBar.HatchMyPxmData.psvSkill[i].psvType,
                psvName = uiMiddleBar.HatchMyPxmData.psvSkill[i].psvName,
                psvRank = uiMiddleBar.PsvData[i].NewPsvRank,
                psvValue = uiMiddleBar.PsvData[i].NewPsvValue
            });
        }
        SaveManager.Instance.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "psvSkill", newPsvs);
    }
}
