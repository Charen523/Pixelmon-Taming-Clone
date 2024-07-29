using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEggLvPopup : UIBase
{
    public Button GaugeUpBtn;
    public Button LvUpBtn;
    public Transform Gauges;
    public LvUpGauge LvUpGauge;
    public TextMeshProUGUI Desc;

    // LvUpGauge를 보관하는 리스트와 사용 순서를 관리하는 큐
    private List<LvUpGauge> lvUpGaugesList = new List<LvUpGauge>();
    private Queue<LvUpGauge> lvUpGaugesQueue = new Queue<LvUpGauge>();

    private string[] descs = { "Lv업 게이지", "Lv업 중" };
    private int gaugeCnt = 1;
    private int price;
    private UserData userData;

    private void Start()
    {
        LvUpGauge gauge1 = Instantiate(LvUpGauge, Gauges);
        LvUpGauge gauge2 = Instantiate(LvUpGauge, Gauges);

        // 리스트와 큐에 추가
        lvUpGaugesList.Add(gauge1);
        lvUpGaugesList.Add(gauge2);

        lvUpGaugesQueue.Enqueue(gauge1);
        lvUpGaugesQueue.Enqueue(gauge2);

        userData = SaveManager.Instance.userData;
    }

    private void OnEnable()
    {
        price = (int)Mathf.Pow(5000, userData.eggLv);
    }

    public void OnClickGaugeUpBtn()
    {
        if (userData.gold >= price)
        {
            // 큐의 앞쪽 요소를 확인하고 처리
            LvUpGauge gauge = lvUpGaugesQueue.Peek();
            if (!gauge.IsFull)
            {
                gauge.GaugeUp();
                // 큐의 앞쪽 요소를 제거하고 다시 추가하여 순서를 유지
                lvUpGaugesQueue.Dequeue();
                lvUpGaugesQueue.Enqueue(gauge);
            }
        }
    }
}
