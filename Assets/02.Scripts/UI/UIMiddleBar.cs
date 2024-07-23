using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
    public PixelmonRank rank;
    public Image HatchedPixelmonImg;
    public PixelmonData HatchPxmData;

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
        // 확률에 따라 픽셀몬 등급 랜덤뽑기
        rank = PerformGacha(eggLv.ToString());

        // 알 색상 변경

        // 등급에 해당하는 픽셀몬 랜덤뽑기
        var data = DataManager.Instance.pixelmonData.data;
        List<PixelmonData> randData = new List<PixelmonData>(data.Count);

        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].rank == rank.ToString())
            {
                randData.Add(data[i]);
            }
        }

        HatchPxmData = randData[UnityEngine.Random.Range(0, randData.Count)];
        HatchedPixelmonImg.sprite = HatchPxmData.icon;
        Debug.Log("[이름] : " + HatchPxmData.name + " [등급] : " + HatchPxmData.rank);

        // 확률에 따라 픽셀몬 능력치 등급 랜덤뽑기

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
        BreakAnim.SetBool(Data.EggBreakParameterHash, true);
        HatchAnim.SetBool(Data.EggHatchParameterHash, true);

        // 애니메이션 끝난지 체크
        float startTime = Time.time;
        while (Time.time - startTime < BreakClip.length) yield return null;

        HatchedPixelmonImg.gameObject.SetActive(true);
        HatchResultPopup.SetActive(true);

        yield return getPixelmon;
        yield return Gacha(); // 다음 알 셋팅
    }

    public PixelmonRank PerformGacha(string rcode)
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
        
        int randNum = UnityEngine.Random.Range(100, 10001);

        float cumProb = 0;
        for (int i = 0; i < probs.Length; i++)
        {
            cumProb += probs[i] * 100;
            if (randNum <= cumProb)
            {
                return (PixelmonRank)i;
            }
        }

        throw new System.Exception("확률 합 != 100");
    }

    public void OnClickGetPixelmon(bool isReplace)
    {
        // 애니메이션 끄기
        BreakAnim.SetBool(Data.EggBreakParameterHash, false);
        HatchAnim.SetBool(Data.EggHatchParameterHash, false);

        HatchedPixelmonImg.gameObject.SetActive(false);
        HatchAnimGO.SetActive(false);
        isGetPixelmon = true;
    }
}
