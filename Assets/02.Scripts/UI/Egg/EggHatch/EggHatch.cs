using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggHatch : MonoBehaviour
{
    #region 애니메이션
    public AnimationData AnimData = new AnimationData();
    public Animator BreakAnim;
    public Animator HatchAnim;
    public GameObject HatchAnimGO;
    public AnimationClip BreakClip;
    #endregion

    #region 알뽑기 결과
    public PixelmonRank Rank;
    public Image HatchedPixelmonImg;
    public PixelmonData HatchPxmData;
    public MyPixelmonData HatchMyPxmData;
    public PxmPsvData[] PsvData;
    public bool IsOwnedPxm;

    private WaitUntil getPixelmon;
    private bool isDoneGetPxm;

    private UIHatchResultPopup HatchResultPopup;
    #endregion

    private UserData userData => SaveManager.Instance.userData;

    private async void Awake()
    {
        HatchResultPopup = await UIManager.Show<UIHatchResultPopup>();
    }

    private void Start()
    {
        getPixelmon = new WaitUntil(() => isDoneGetPxm == true);
        HatchedPixelmonImg.gameObject.SetActive(false);

        AnimData.Initialize();

        PsvData = new PxmPsvData[4];
        for (int i = 0; i < PsvData.Length; i++)
            PsvData[i] = new PxmPsvData();

        if (!userData.isGetPxm)
        {
            IsOwnedPxm = userData.isOwnedPxm;
            HatchPxmData = userData.hatchPxmData;
            HatchMyPxmData = userData.hatchMyPxmData;
            PsvData = userData.psvData;
            Rank = (PixelmonRank)Enum.Parse(typeof(PixelmonRank), HatchPxmData.rank);
            StartCoroutine(SetPxmHatchAnim());
        }
    }

    private bool Gacha()
    {
        #region 확률에 따라 픽셀몬 등급 랜덤뽑기
        Rank = PerformPxmGacha(userData.eggLv.ToString());

        // 등급에 해당하는 픽셀몬 랜덤뽑기
        var pxmData = DataManager.Instance.pixelmonData.data;
        List<PixelmonData> randPxmData = new List<PixelmonData>(pxmData.Count);

        for (int i = 0; i < pxmData.Count; i++)
        {
            if (pxmData[i].rank == Rank.ToString())
            {
                randPxmData.Add(pxmData[i]);
            }
        }

        HatchPxmData = randPxmData[UnityEngine.Random.Range(0, randPxmData.Count)];
        SaveManager.Instance.SetFieldData(nameof(userData.hatchPxmData), HatchPxmData);
        #endregion

        #region 픽셀몬 능력치 랜덤뽑기
        IsOwnedPxm = false;
        foreach (var data in userData.ownedPxms)
        {
            if (HatchPxmData.rcode == data.rcode)
            {
                IsOwnedPxm = true;
                HatchMyPxmData = data;
                SaveManager.Instance.SetFieldData(nameof(userData.hatchMyPxmData), HatchMyPxmData);
                break;
            }
        }
        if (IsOwnedPxm)
            SetNewPsvValue();
        else SetFirstPsvValue();
        SaveManager.Instance.SetFieldData(nameof(userData.psvData), PsvData);
        SaveManager.Instance.SetFieldData(nameof(userData.isOwnedPxm), IsOwnedPxm);
        #endregion
        return true;
    }
    private void SetNewPsvValue()
    {
        for (int i = 0; i < HatchMyPxmData.psvSkill.Count; i++)
        {
            var psvData = DataManager.Instance.GetData<BasePsvData>(HatchMyPxmData.psvSkill[i].psvName);
            var randAbility = RandAbilityUtil.PerformAbilityGacha(HatchMyPxmData.psvSkill[i].psvType, psvData.maxRate, HatchMyPxmData.psvSkill[i].psvValue);
            PsvData[i].NewPsvRank = randAbility.AbilityRank;
            PsvData[i].NewPsvValue = randAbility.AbilityValue;
        }
    }

    private void SetFirstPsvValue()
    {
        var basePsvData = RandAbilityUtil.RandAilityData();
        var randAbility = RandAbilityUtil.PerformAbilityGacha((AbilityType)basePsvData.psvEnum, basePsvData.maxRate);
        PsvData[0].PsvType = (AbilityType)basePsvData.psvEnum;
        PsvData[0].PsvName = basePsvData.rcode;
        PsvData[0].NewPsvRank = randAbility.AbilityRank;
        PsvData[0].NewPsvValue = randAbility.AbilityValue;
    }

    public void OnClickEgg()
    {
        StartCoroutine(ClickEgg());
    }

    public IEnumerator ClickEgg()
    {
        if (userData.eggCount <= 0) yield break;

        if (userData.isGetPxm)
        {
            SaveManager.Instance.SetFieldData(nameof(userData.eggCount), -1, true);
            Gacha();
        }
            
        isDoneGetPxm = false;

        if (userData.isGetPxm == true)
            yield return SetPxmHatchAnim();

        HatchResultPopup.SetActive(true);
        HatchResultPopup.SetPopup(this);

        yield return getPixelmon;
    }

    private IEnumerator SetPxmHatchAnim()
    {
        HatchAnimGO.SetActive(true);

        // 애니메이션 실행
        BreakAnim.SetInteger(AnimData.EggBreakParameterHash, (int)Rank);
        HatchAnim.SetBool(AnimData.EggHatchParameterHash, true);

        // 애니메이션 끝난지 체크
        float startTime = Time.time;
        while (Time.time - startTime < BreakClip.length + 0.08f) yield return null;

        HatchedPixelmonImg.gameObject.SetActive(true);
        HatchedPixelmonImg.sprite = HatchPxmData.icon;
    }

    public PixelmonRank PerformPxmGacha(string rcode)
    {
        var data = DataManager.Instance.GetData<EggRateData>(rcode);

        float[] probs = { data.common, data.advanced, data.rare, data.epic, data.legendary };

        #region 확률 합이 100인지 체크
        float totalProb = 0;
        foreach (float prob in probs)
        {
            totalProb += prob;
        }
        if (totalProb != 100)
        {
            Debug.LogError("확률 합 != 100");
        }
        #endregion

        int randProb = UnityEngine.Random.Range(100, 10001);

        float cumProb = 0;
        for (int i = 0; i < probs.Length; i++)
        {
            cumProb += probs[i] * 100;
            if (randProb <= cumProb)
            {
                return (PixelmonRank)i;
            }
        }

        throw new System.Exception("확률 합 != 100");
    }

    public void GetPixelmon()
    {
        BreakAnim.SetInteger(AnimData.EggBreakParameterHash, -1);
        HatchAnim.SetBool(AnimData.EggHatchParameterHash, false);

        HatchedPixelmonImg.gameObject.SetActive(false);
        HatchAnimGO.SetActive(false);

        SaveManager.Instance.SetFieldData(nameof(userData.isGetPxm), true);
        isDoneGetPxm = true;
    }
}
