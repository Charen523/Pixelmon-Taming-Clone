using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [SerializeField] private GameObject CollectBtn;
    [SerializeField] private GameObject OwnedBtn;
    [SerializeField] private Button rePlaceBtn;

    private EggHatch eggHatch;
    private UserData userData => SaveManager.Instance.userData;
    
    public void SetPopup(EggHatch eggHatch)
    {
        SaveManager.Instance.SetFieldData(nameof(userData.isGetPxm), false);
        this.eggHatch = eggHatch;
        #region 소환된 픽셀몬 정보 UI        
        pxmName.text = eggHatch.HatchPxmData.name;
        rateTxt.text = UIUtils.TranslateRank(eggHatch.Rank);
        rateBg.sprite = PxmRankImgUtil.GetRankImage(eggHatch.Rank, rateBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonBg.sprite = PxmRankImgUtil.GetRankImage(eggHatch.Rank, pixelmonBgs.ConvertAll<BaseBg>(bg => (BaseBg)bg));
        pixelmonImg.sprite = eggHatch.HatchedPixelmonImg.sprite;
        #endregion

        #region 이미 가지고 있는 픽셀몬인지 체크 & 픽셀몬 능력치 UI 셋팅
        AtkValueTxt.text = eggHatch.HatchPxmData.basePerAtk.ToString("F2");
        TraitTypeTxt.text = eggHatch.HatchPxmData.trait.TranslateTraitString();
        TraitValueTxt.text = eggHatch.HatchPxmData.traitValue.ToString("F2");
        OwnHpValueTxt.text = eggHatch.HatchPxmData.basePerHp.ToString("F2");
        OwnDefenseValueTxt.text = eggHatch.HatchPxmData.basePerDef.ToString("F2");

        if (eggHatch.IsOwnedPxm)
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
        CollectBtn.SetActive(false);
        OwnedBtn.SetActive(true);

        UIPsv[0].NewPsvRankTxt.gameObject.SetActive(true);
        UIPsv[0].OldPsvValueTxt.gameObject.SetActive(true);
        UIPsv[0].ArrowImg.gameObject.SetActive(true);
        for (int i = 0; i < eggHatch.HatchMyPxmData.psvSkill.Count; i++)
        {
            UIPsv[i].gameObject.SetActive(true);

            UIPsv[i].OldPsvRankTxt.text = eggHatch.HatchMyPxmData.psvSkill[i].psvRank;
            UIPsv[i].PsvNameTxt.text = eggHatch.HatchMyPxmData.psvSkill[i].psvName;
            UIPsv[i].OldPsvValueTxt.text = new StringBuilder().Append(eggHatch.HatchMyPxmData.psvSkill[i].psvValue.ToString("F2")).Append('%').ToString();

            UIPsv[i].NewPsvRankTxt.text = eggHatch.PsvData[i].NewPsvRank.ToString();
            UIPsv[i].NewPsvValueTxt.text = new StringBuilder().Append(eggHatch.PsvData[i].NewPsvValue.ToString("F2")).Append('%').ToString();
            if (eggHatch.PsvData[i].NewPsvValue > eggHatch.HatchMyPxmData.psvSkill[i].psvValue)
                UIPsv[i].NewPsvValueTxt.HexColor("#78FF1E");
            else if(eggHatch.PsvData[i].NewPsvValue < eggHatch.HatchMyPxmData.psvSkill[i].psvValue)
                UIPsv[i].NewPsvValueTxt.HexColor("#FF0A0A");
        }
        for (int i = 3; i >= eggHatch.HatchMyPxmData.psvSkill.Count; i--)
        {
            UIPsv[i].gameObject.SetActive(false);
        }
    }

    private void FirstPxmUI()
    {
        CollectBtn.SetActive(true);
        OwnedBtn.SetActive(false);

        UIPsv[0].PsvNameTxt.text = eggHatch.PsvData[0].PsvName;
        UIPsv[0].OldPsvRankTxt.text = eggHatch.PsvData[0].NewPsvRank.ToString();
        UIPsv[0].NewPsvValueTxt.text = new StringBuilder().Append(eggHatch.PsvData[0].NewPsvValue.ToString("F2")).Append('%').ToString();
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
        if (isReplaceBtn && eggHatch.IsOwnedPxm == true) // 교체하기(교체 및 수집)
        {
            ReplacePsv();
        }
        else // 수집하기
        {
            if (!eggHatch.IsOwnedPxm)                
                GetFirst();
            else
                GetRepetition();               
        }
        eggHatch.GetPixelmon();

        SetActive(false);
    }

    private void GetFirst()
    {
        #region Init Value
        List<PsvSkill> firstPsv = new List<PsvSkill>();
        firstPsv.Add(new PsvSkill
        {
            psvType = eggHatch.PsvData[0].PsvType,
            psvName = eggHatch.PsvData[0].PsvName,
            psvRank = eggHatch.PsvData[0].NewPsvRank,
            psvValue = eggHatch.PsvData[0].NewPsvValue
        });

        float[] ownEffectValue = { eggHatch.HatchPxmData.basePerHp, eggHatch.HatchPxmData.basePerDef };
        #endregion

        #region Update Value
        SaveManager.Instance.UpdatePixelmonData(eggHatch.HatchPxmData.id, "rcode", eggHatch.HatchPxmData.rcode);
        SaveManager.Instance.UpdatePixelmonData(eggHatch.HatchPxmData.id, "id", eggHatch.HatchPxmData.id);
        SaveManager.Instance.UpdatePixelmonData(eggHatch.HatchPxmData.id, "isOwned", true);
        SaveManager.Instance.UpdatePixelmonData(eggHatch.HatchPxmData.id, "atkValue", eggHatch.HatchPxmData.basePerAtk);
        SaveManager.Instance.UpdatePixelmonData(eggHatch.HatchPxmData.id, "psvSkill", firstPsv);
        SaveManager.Instance.UpdatePixelmonData(eggHatch.HatchPxmData.id, "ownEffectValue", ownEffectValue);
        PixelmonManager.Instance.UnLockedPixelmon(eggHatch.HatchPxmData.id);
        #endregion
    }

    private void GetRepetition()
    {
        SaveManager.Instance.UpdatePixelmonData(eggHatch.HatchPxmData.id, "evolvedCount", ++userData.ownedPxms[eggHatch.HatchPxmData.id].evolvedCount);
        PixelmonManager.Instance.UnLockedPixelmon(eggHatch.HatchPxmData.id);
    }

    private void ReplacePsv()
    {
        List<PsvSkill> newPsvs = new List<PsvSkill>();
        for (int i = 0; i < eggHatch.HatchMyPxmData.psvSkill.Count; i++)
        {
            newPsvs.Add(new PsvSkill
            {
                psvType = eggHatch.HatchMyPxmData.psvSkill[i].psvType,
                psvName = eggHatch.HatchMyPxmData.psvSkill[i].psvName,
                psvRank = eggHatch.PsvData[i].NewPsvRank,
                psvValue = eggHatch.PsvData[i].NewPsvValue
            });
        }
        SaveManager.Instance.UpdatePixelmonData(eggHatch.HatchPxmData.id, "psvSkill", newPsvs);
        PixelmonManager.Instance.ApplyStatus(PixelmonManager.Instance.pxmTab.allData[eggHatch.HatchPxmData.id].pxmData, PixelmonManager.Instance.pxmTab.allData[eggHatch.HatchPxmData.id].myPxmData);
    }
}
