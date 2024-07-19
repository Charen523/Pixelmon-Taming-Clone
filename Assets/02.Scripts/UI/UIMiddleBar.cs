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
    public AnimationData Data = new AnimationData();
    public Animator BreakAnim;
    public Animator HatchAnim;
    public GameObject HatchResultPanel;
    public Image HatchedPixelmonImg;
    public Button GetPixelmonBtn;

    public int eggCount = 100;
    public int eggLv = 1;

    public bool isHatchedPixelmon;
    public bool isGetPixelmon;
    public PixelmonRank rank;

    private readonly string eggRcode = "EGG";

    private bool isBreakAnimationEnded;
    private bool isHatchAnimationEnded;
    #endregion

    private void Start()
    {
        //eggCount = InventoryManager.Instance.userData.eggCount;
        //eggLv = InventoryManager.Instance.userData.eggLv;
        EggCntText.text = eggCount.ToString();
        EggLvText.text = eggLv.ToString();

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

        if (isHatchedPixelmon) // 알 이미 깠음
        {
            HatchResultPanel.SetActive(true);
        }
        else // 알 안깠음
        {
            // 애니메이션 실행
            BreakAnim.SetBool(Data.EggBreakParameterHash, true);
            HatchAnim.SetBool(Data.EggHatchParameterHash, true);

            // 화면에 표시(HatchedPixelmonImg)
            Debug.Log($"뽑은 픽셀몬 등급(ClickEgg) : {rank}");

            // 애니메이션 이벤트가 끝날 때까지 대기
            //while (!isBreakAnimationEnded)
            //{
            //    yield return null;
            //}

            //while (!isHatchAnimationEnded)
            //{
            //    yield return null;
            //}

            //isBreakAnimationEnded = false;
            //isHatchAnimationEnded = false;

            yield return new WaitForSeconds(1.0f);

            HatchResultPanel.SetActive(true);

            if (isGetPixelmon)
            {
                isHatchedPixelmon = true;
                yield return Gacha(); // 다음 알 셋팅
            }
            else
            {
                isHatchedPixelmon = false;
            }
        }
    }

    public void OnBreakAnimationEnd()
    {
        isBreakAnimationEnded = true;
    }

    public void OnHatchAnimationEnded()
    {
        isHatchAnimationEnded = true;
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

    public void OnClickGetPixelmon()
    {       
        // 인벤토리에 넣어주기

        // 애니메이션 끄기
        BreakAnim.SetBool(Data.EggBreakParameterHash, false);
        HatchAnim.SetBool(Data.EggHatchParameterHash, false);

        isGetPixelmon = true;
        HatchResultPanel.SetActive(false);
    }
}
