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
    #region 알 뽑기 필드
    #region 소환 레벨, 알 개수
    public TextMeshProUGUI EggLvText;
    public TextMeshProUGUI EggCntText;
    public int eggCount = 100;
    public int eggLv = 1;
    #endregion
    #region 애니메이션
    public AnimationData Data = new AnimationData();
    public Animator BreakAnim;
    public Animator HatchAnim;
    public GameObject HatchAnimGO;
    public AnimationClip BreakClip;
    #endregion
    public PixelmonRank Rank;
    public Image HatchedPixelmonImg;
    public PixelmonData HatchPxmData;

    private UIHatchResultPopup HatchResultPopup;
    private UIEggLvPopup EggLvPopup;
    private WaitUntil getPixelmon;
    private bool isGetPixelmon;
    #endregion

    private async void Awake()
    {
        HatchResultPopup = await UIManager.Show<UIHatchResultPopup>();
        EggLvPopup = await UIManager.Show<UIEggLvPopup>();
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
        HatchedPixelmonImg.sprite = HatchPxmData.icon;
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
        BreakAnim.SetInteger(Data.EggBreakParameterHash, (int)Rank);
        HatchAnim.SetBool(Data.EggHatchParameterHash, true);

        // 애니메이션 끝난지 체크
        float startTime = Time.time;
        while (Time.time - startTime < BreakClip.length) yield return null;

        HatchedPixelmonImg.gameObject.SetActive(true);
        HatchResultPopup.SetActive(true);
        HatchResultPopup.SetPopup();

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

    public void OnClickGetPixelmon(bool isReplace)
    {
        BreakAnim.SetInteger(Data.EggBreakParameterHash, -1);
        HatchAnim.SetBool(Data.EggHatchParameterHash, false);
        
        HatchedPixelmonImg.gameObject.SetActive(false);
        HatchAnimGO.SetActive(false);
        isGetPixelmon = true;
    }

    public void OnClickEggLvBtn()
    {
        EggLvPopup.SetActive(true);
    }

    public void OnClickAutoBtn()
    {
    }
}
