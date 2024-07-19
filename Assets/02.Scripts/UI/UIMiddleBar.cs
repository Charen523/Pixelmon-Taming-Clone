using System;
using System.Text;
using TMPro;
using UnityEngine;


public class UIMiddleBar : UIBase
{
    [Header("알 뽑기")]
    public TextMeshProUGUI eggLvText;
    public TextMeshProUGUI eggCountText;
    public Animator eggAnim;
    public GameObject EffectAnim;

    public int eggCount = 100;
    public int eggLv = 1;

    private readonly string eggRcode = "EGG";

    private void Start()
    {
        // UserData 받아와서 ㄱㄱ
        //aggCount = 100;
        //aggLv = 1; 
        eggCountText.text = eggCount.ToString();
        eggLvText.text = eggLv.ToString();
    }

    public void OnClickEgg()
    { 
        if(eggCount > 0) // 코루틴으로 해야할듯
        {
            // 애니메이션 실행
            // 확률에 따라 픽셀몬 랜덤뽑기
            string levelString = eggLv.ToString("D5"); // 5자리 숫자로 변환, 빈 자리는 0으로 채움

            StringBuilder sb = new StringBuilder();
            sb.Append(eggRcode);
            sb.Append(levelString);
            string rcode = sb.ToString();
            Draw(rcode);

            // 등장 애니메이션 실행
            // EffectAnim.SetActive(true);
            // 인벤토리에 넣어주기

            // 애니메이션 끄기

        }
    }

    public String Draw(string rcode)
    {
        var data = DataManager.Instance.GetData<EggRateData>(rcode);

        float[] probs = { data.common, data.advanced, data.rare, data.epic, data.legendary };

        // Check if probabilities sum to 100
        float totalProb = 0;
        foreach (float prob in probs)
        {
            totalProb += prob;
        }
        if (totalProb != 100)
        {
            throw new InvalidOperationException("Probabilities must sum to 100.");
        }

        System.Random rnd = new System.Random();
        int randNum = rnd.Next(1, 101); // 1부터 100까지의 난수를 생성

        float cumProb = 0;
        for (int i = 0; i < probs.Length; i++)
        {
            cumProb += probs[i];
            if (randNum <= cumProb)
            {
                return i.ToString();
            }
        }
        return null; // probabilities 배열의 모든 값의 합이 100이 아닌 경우
    }

}
