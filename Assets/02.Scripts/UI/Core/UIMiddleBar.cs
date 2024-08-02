using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EggImg : BaseBg
{
}
public class UIMiddleBar : UIBase
{
    #region 알 뽑기 필드
    #region 소환 레벨, 알 개수
    public TextMeshProUGUI EggLvText;
    public TextMeshProUGUI EggCntText;
    public int eggCount = 100;
    public int eggLv = 1;
    #endregion
    #region 애니메이션
    public AnimationData AnimData = new AnimationData();
    public Animator BreakAnim;
    public Animator HatchAnim;
    public GameObject HatchAnimGO;
    public AnimationClip BreakClip;
    #endregion
    public PixelmonRank Rank;
    public Image HatchedPixelmonImg;
    public PixelmonData HatchPxmData;
    public MyPixelmonData HatchMyPxmData;
    public PxmPsvData[] PsvData;
    public bool IsOwnedPxm;

    private UIHatchResultPopup HatchResultPopup;
    private UIEggLvPopup EggLvPopup;
    private WaitUntil getPixelmon;
    private bool isDoneGetPxm;

    private UserData userData => SaveManager.Instance.userData;
    private Coroutine hatchedCoroutine;
    #endregion

    private async void Awake()
    {
        HatchResultPopup = await UIManager.Show<UIHatchResultPopup>();
        EggLvPopup = await UIManager.Show<UIEggLvPopup>();
    }
    private void Start()
    {
        eggCount = userData.eggCount;
        eggLv = userData.eggLv;
        EggCntText.text = eggCount.ToString();
        EggLvText.text = eggLv.ToString();
  
        getPixelmon = new WaitUntil(() => isDoneGetPxm == true);
        HatchedPixelmonImg.gameObject.SetActive(false);

        AnimData.Initialize();

        PsvData = new PxmPsvData[4];
        for (int i = 0; i < PsvData.Length; i++)
            PsvData[i] = new PxmPsvData();

        if (!userData.isGetPxm)
        {
            HatchPxmData = userData.hatchPxmData;
            HatchMyPxmData = userData.hatchMyPxmData;
            PsvData = userData.psvData;
            hatchedCoroutine = StartCoroutine(SetPxmHatchAnim());
        }
    }

    private bool Gacha()
    {
        #region 확률에 따라 픽셀몬 등급 랜덤뽑기
        Rank = PerformPxmGacha(eggLv.ToString());

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
        SaveManager.Instance.SetData(nameof(userData.hatchPxmData), HatchPxmData);
        #endregion

        #region 픽셀몬 능력치 랜덤뽑기
        IsOwnedPxm = false;
        foreach (var data in userData.ownedPxms)
        {
            if (HatchPxmData.rcode == data.rcode)
            {
                IsOwnedPxm = true;
                HatchMyPxmData = data;
                SaveManager.Instance.SetData(nameof(userData.hatchMyPxmData), HatchMyPxmData);
                break;
            }
        }
        if (IsOwnedPxm)
            SetNewPsvValue();
        else SetFirstPsvValue();
        SaveManager.Instance.SetData(nameof(userData.psvData), PsvData);
        #endregion
        return true;
    }
    private void SetNewPsvValue()
    {
        for (int i = 0; i < HatchMyPxmData.psvSkill.Count; i++)
        { 
            var psvData = DataManager.Instance.GetData<BasePsvData>(HatchMyPxmData.psvSkill[i].psvName);
            var randAbility = RandAbilityUtil.PerformAbilityGacha(HatchMyPxmData.psvSkill[i].psvType, psvData.maxRate);
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
        if (userData.isGetPxm)
            Gacha();
        StartCoroutine(ClickEgg());
    }

    public IEnumerator ClickEgg()
    {
        if (eggCount <= 0) yield break;

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
        while (Time.time - startTime < BreakClip.length) yield return null;

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

        SaveManager.Instance.SetData(nameof(userData.isGetPxm), true);
        isDoneGetPxm = true;
    }

    public void OnClickEggLvBtn()
    {
        EggLvPopup.SetActive(true);
        EggLvPopup.SetPopup();
    }

    public void OnClickAutoBtn()
    {
    }

    private void OnDisable()
    {
        if (hatchedCoroutine != null)
        {
            StopCoroutine(hatchedCoroutine);
            hatchedCoroutine = null;
        }
    }
}
