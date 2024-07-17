using System;
using System.Text;
using TMPro;
using UnityEngine;


public class UIMiddleBar : UIBase
{
    [Header("알 뽑기")]
    public TextMeshProUGUI aggLvText;
    public TextMeshProUGUI aggCountText;
    public Animator aggAnim;
    public GameObject EffectAnim;

    public int aggCount = 100;
    public int aggLv = 1;

    private readonly string eggRcode = "EGG";

    private void Start()
    {
        // UserData 받아와서 ㄱㄱ
        //aggCount = 100;
        //aggLv = 1; 
        aggCountText.text = aggCount.ToString();
        aggLvText.text = aggLv.ToString();
    }

    public void OnClickAgg()
    { 
        if(aggCount > 0) // 코루틴으로 해야할듯
        {
            // 애니메이션 실행
            // 확률에 따라 픽셀몬 랜덤뽑기
            string levelString = aggLv.ToString("D5"); // 5자리 숫자로 변환, 빈 자리는 0으로 채움

            StringBuilder sb = new StringBuilder();
            sb.Append(eggRcode);
            sb.Append(levelString);
            string rcode = sb.ToString();
            DataManager.Instance.GetData<EggRateData>(rcode);

            // 등장 애니메이션 실행
            // EffectAnim.SetActive(true);
            // 인벤토리에 넣어주기

            // 애니메이션 끄기

        }
    }

    public string Draw(string[] rarities, int[] probabilities)
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(1, 101); // 1부터 100까지의 난수를 생성

        int cumulativeProbability = 0;
        for (int i = 0; i < rarities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomNumber <= cumulativeProbability)
            {
                return rarities[i];
            }
        }
        return null; // 여기에 도달할 경우 없음
    }
}
