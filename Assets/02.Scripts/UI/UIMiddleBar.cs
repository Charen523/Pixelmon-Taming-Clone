using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
    public Image HatchedPixelmonImg;
    private UIBase HatchResultPopup;
    private PixelmonRank rank;

    private WaitUntil getPixelmon;
    private bool isGetPixelmon;

    private readonly string eggRcode = "EGG";
    #endregion

    private async void Awake()
    {
        HatchResultPopup = await UIManager.Show<UIHatchResultPopup>();
    }
    private void Start()
    {
        //eggCount = InventoryManager.Instance.userData.eggCount;
        //eggLv = InventoryManager.Instance.userData.eggLv;
        EggCntText.text = eggCount.ToString();
        EggLvText.text = eggLv.ToString();

        getPixelmon = new WaitUntil(() => isGetPixelmon == true);

        Data.Initialize();
        Gacha();
    }

    private bool Gacha()
    {
        // 확률에 따라 등급 랜덤뽑기
        string levelString = eggLv.ToString("D5"); // 5자리 숫자로 변환, 빈 자리는 0으로 채움
        StringBuilder sb = new StringBuilder();
        sb.Append(eggRcode);
        sb.Append(levelString);
        string rcode = sb.ToString();
        rank = PerformGacha(rcode);
        Debug.Log($"뽑은 픽셀몬 등급(Gacha) : {rank}");

        // 등급에 해당하는 픽셀몬 랜덤뽑기

        // 알 색상 변경 

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

        System.Random rnd = new System.Random();
        int randNum = rnd.Next(100, 10001); // 1부터 100까지의 난수를 생성

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
        if (isReplace) // 교체하기
        {
            Debug.Log("ReplaceBtn");
        }
        else // 수집하기
        {
            Debug.Log("CollectBtn");
        }

        // 애니메이션 끄기
        BreakAnim.SetBool(Data.EggBreakParameterHash, false);
        HatchAnim.SetBool(Data.EggHatchParameterHash, false);

        HatchAnimGO.SetActive(false);
        isGetPixelmon = true;
    }
}
