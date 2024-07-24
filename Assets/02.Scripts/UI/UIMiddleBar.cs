using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EggImg : BaseBg
{
}
public class UIMiddleBar : UIBase
{
    #region 알 뽑기
    [Header("알 뽑기")]
    public TextMeshProUGUI EggLvText;
    public TextMeshProUGUI EggCntText;
    public int eggCount = 100;
    public int eggLv = 1;
    #region 알 뽑기 애니메이션
    public AnimationData Data = new AnimationData();
    public Animator BreakAnim;
    public Animator HatchAnim;
    public GameObject HatchAnimGO;
    public AnimationClip BreakClip;
    #endregion
    public Image HatchedPixelmonImg;
    public PixelmonRank rank;
    public PixelmonData HatchPxmData;
    public Dictionary<string, Tuple<string, float>> AbilityDic = new Dictionary<string, Tuple<string, float>>();

    private UIBase HatchResultPopup;
    private WaitUntil getPixelmon;
    private bool isGetPixelmon;
    #endregion

    private async void Awake()
    {
        HatchResultPopup = await UIManager.Show<UIHatchResultPopup>();
    }
    private void Start()
    {
        eggCount = SaveManager.Instance.userData.eggCount;
        eggLv = SaveManager.Instance.userData.eggLv;
        EggCntText.text = eggCount.ToString();
        EggLvText.text = eggLv.ToString();
  
        getPixelmon = new WaitUntil(() => isGetPixelmon == true);
        HatchedPixelmonImg.gameObject.SetActive(false);

        Data.Initialize();
        Gacha();
    }

    private bool Gacha()
    {
        #region 확률에 따라 픽셀몬 등급 랜덤뽑기
        rank = PerformPxmGacha(eggLv.ToString());

        // 등급에 해당하는 픽셀몬 랜덤뽑기
        var pxmData = DataManager.Instance.pixelmonData.data;
        List<PixelmonData> randPxmData = new List<PixelmonData>(pxmData.Count);

        for (int i = 0; i < pxmData.Count; i++)
        {
            if (pxmData[i].rank == rank.ToString())
            {
                randPxmData.Add(pxmData[i]);
            }
        }

        HatchPxmData = randPxmData[UnityEngine.Random.Range(0, randPxmData.Count)];
        HatchedPixelmonImg.sprite = HatchPxmData.icon;
        #endregion

        #region 확률에 따라 픽셀몬 능력치 등급 랜덤뽑기
        // 공격력
        var gachaResult = PerformAbilityGacha();
        float atkResult = HatchPxmData.baseAtk * ((float)gachaResult.RandValue / 100);
        AbilityDic.Add("Attack", Tuple.Create(gachaResult.DropRcode, atkResult));
        
        // 패시브

        // 보유 효과
        #endregion

        return true;
    }

    public void OnClickEgg()
    {
        StartCoroutine(ClickEgg());
    }

    public IEnumerator ClickEgg()
    {
        if (eggCount <= 0) yield break;

        HatchAnimGO.SetActive(true);
        isGetPixelmon = false;

        // 애니메이션 실행
        BreakAnim.SetInteger(Data.EggBreakParameterHash, (int)rank);
        HatchAnim.SetBool(Data.EggHatchParameterHash, true);

        // 애니메이션 끝난지 체크
        float startTime = Time.time;
        while (Time.time - startTime < BreakClip.length) yield return null;

        HatchedPixelmonImg.gameObject.SetActive(true);
        HatchResultPopup.SetActive(true);

        yield return getPixelmon;
        yield return Gacha(); // 다음 알 셋팅
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

    public (string DropRcode, int RandValue) PerformAbilityGacha()
    {
        var data = DataManager.Instance.abilityRateData.data;

        #region 확률 합이 100인지 체크
        float totalProb = 0;
        foreach (var prob in data)
        {
            totalProb += prob.dropRate;
        }
        if (totalProb != 100)
        {
            Debug.LogError("확률 합 != 100");
            return (null, 0); // 확률 합이 100이 아닐 때 null과 0 반환
        }
        #endregion

        #region 능력치 등급 랜덤(rocde)
        int randProb = UnityEngine.Random.Range(100, 10001);
        float cumProb = 0;
        string dropRcode = null;

        foreach (var prob in data)
        {
            cumProb += prob.dropRate * 100;
            if (randProb <= cumProb)
            {
                dropRcode = prob.rcode;
                break;
            }
        }
        #endregion

        #region 능력치값 랜덤(min~max)
       
        var dropData = DataManager.Instance.GetData<AbilityRateData>(dropRcode);
        int randValue = UnityEngine.Random.Range(dropData.min, dropData.max + 1);
        Debug.Log(randValue);
        #endregion

        return (dropRcode, randValue);
    }

    public void OnClickGetPixelmon(bool isReplace)
    {
        BreakAnim.SetInteger(Data.EggBreakParameterHash, -1);
        HatchAnim.SetBool(Data.EggHatchParameterHash, false);
        
        HatchedPixelmonImg.gameObject.SetActive(false);
        HatchAnimGO.SetActive(false);
        AbilityDic.Clear();
        isGetPixelmon = true;
    }
}
