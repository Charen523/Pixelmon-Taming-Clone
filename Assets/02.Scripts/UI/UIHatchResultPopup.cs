using System;
using System.Collections.Generic;
using System.Text;
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
    [SerializeField] private TextMeshProUGUI AtkValueTxt;
    [SerializeField] private TextMeshProUGUI TraitTypeTxt;
    [SerializeField] private TextMeshProUGUI TraitValueTxt;

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
    
    public void SetPopup()
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
        AtkValueTxt.text = uiMiddleBar.HatchPxmData.basePerAtk.ToString("F2");
        TraitTypeTxt.text = uiMiddleBar.HatchPxmData.trait.TranslateTraitString();
        TraitValueTxt.text = uiMiddleBar.HatchPxmData.traitValue.ToString("F2");
        OwnHpValueTxt.text = uiMiddleBar.HatchPxmData.basePerHp.ToString("F2");
        OwnDefenseValueTxt.text = uiMiddleBar.HatchPxmData.basePerDef.ToString("F2");

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
            SetNewPsvValue();
            OwnedPxmUI();
        }
        else
        {
            rePlaceBtn.gameObject.SetActive(false);
            SetFirstPsvValue();
            FirstPxmUI();
        }
        #endregion
    }

    private void SetNewPsvValue()
    {
        for (int i = 0; i < hatchedPxmData.psvSkill.Count; i++)
        {
            var psvData = DataManager.Instance.GetData<BasePsvData>(hatchedPxmData.psvSkill[i].psvName);
            var randAbility = RandAbilityUtil.PerformAbilityGacha(hatchedPxmData.psvSkill[i].psvType, psvData.maxRate);
            Psv[i].NewPsvRank = randAbility.AbilityRank;
            Psv[i].NewPsvValue = randAbility.AbilityValue;
        }
    }

    private void OwnedPxmUI()
    {
        Psv[0].NewPsvRankTxt.gameObject.SetActive(true);
        Psv[0].OldPsvValueTxt.gameObject.SetActive(true);
        Psv[0].ArrowImg.gameObject.SetActive(true);
        for (int i = 0; i <= hatchedPxmData.psvSkill.Count; i++)
        {
            Psv[i].gameObject.SetActive(true);

            Psv[i].OldPsvRankTxt.text = hatchedPxmData.psvSkill[i].psvRank;
            Psv[i].PsvNameTxt.text = hatchedPxmData.psvSkill[i].psvName;
            Psv[i].OldPsvValueTxt.text = new StringBuilder().Append(hatchedPxmData.psvSkill[i].psvValue.ToString("F2")).Append('%').ToString();

            Psv[i].NewPsvRankTxt.text = Psv[i].NewPsvRank.ToString();
            Psv[i].NewPsvValueTxt.text = new StringBuilder().Append(Psv[i].NewPsvValue.ToString("F2")).Append('%').ToString();
        }
    }

    private void SetFirstPsvValue()
    {
        var basePsvData = RandAbilityUtil.RandAilityData();
        var randAbility = RandAbilityUtil.PerformAbilityGacha((AbilityType)basePsvData.psvEnum, basePsvData.maxRate);
        Psv[0].PsvType = (AbilityType)basePsvData.psvEnum;
        Psv[0].PsvName = basePsvData.rcode;
        Psv[0].NewPsvRank = randAbility.AbilityRank;
        Psv[0].NewPsvValue = randAbility.AbilityValue;
    }
    private void FirstPxmUI()
    {
        Psv[0].PsvNameTxt.text = Psv[0].PsvName;
        Psv[0].OldPsvRankTxt.text = Psv[0].NewPsvRank.ToString();
        Psv[0].NewPsvValueTxt.text = new StringBuilder().Append(Psv[0].NewPsvValue.ToString("F2")).Append('%').ToString();

        Psv[0].NewPsvRankTxt.gameObject.SetActive(false);
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
            ReplacePsv();
        }
        else // 수집하기
        {
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
        #region Init Value
        List<PsvSkill> firstPsv = new List<PsvSkill>();
        firstPsv.Add(new PsvSkill
        {
            psvType = Psv[0].PsvType,
            psvName = Psv[0].PsvName,
            psvRank = Psv[0].NewPsvRank,
            psvValue = Psv[0].NewPsvValue
        });

        float[] ownEffectValue = { uiMiddleBar.HatchPxmData.basePerHp, uiMiddleBar.HatchPxmData.basePerDef };
        #endregion

        #region Update Value
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "rcode", uiMiddleBar.HatchPxmData.rcode);
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "id", uiMiddleBar.HatchPxmData.id);
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "isOwned", true);
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "atkValue", uiMiddleBar.HatchPxmData.basePerAtk);
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "traitType", uiMiddleBar.HatchPxmData.trait.TranslateTraitString());
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "traitValue", uiMiddleBar.HatchPxmData.traitValue);
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "psvSkill", firstPsv);
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "ownEffectValue", ownEffectValue);
        PixelmonManager.Instance.UnLockedPixelmon(uiMiddleBar.HatchPxmData.id);
        #endregion
    }

    private void GetRepetition()
    {
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "evolvedCount", ++userData.ownedPxms[uiMiddleBar.HatchPxmData.id].evolvedCount);
    }

    private void ReplacePsv()
    {
        List<PsvSkill> newPsvs = new List<PsvSkill>();
        for (int i = 0; i < hatchedPxmData.psvSkill.Count; i++)
        {
            newPsvs.Add(new PsvSkill
            {
                psvType = hatchedPxmData.psvSkill[i].psvType,
                psvName = hatchedPxmData.psvSkill[i].psvName,
                psvRank = Psv[i].NewPsvRank,
                psvValue = Psv[i].NewPsvValue
            });
        }
        saveManager.UpdatePixelmonData(uiMiddleBar.HatchPxmData.id, "psvSkill", newPsvs);
    }
}
