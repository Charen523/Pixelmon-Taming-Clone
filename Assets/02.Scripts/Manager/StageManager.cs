using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("소환")]
    [SerializeField]
    private Spawner spawner;
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI stageTitleTxt;
    [SerializeField]
    private Image clearBar;
    [SerializeField]
    private TextMeshProUGUI clearTxt;

    [Header("난이도")]
    [SerializeField]
    private string stgRcode;

    [SerializeField]
    private StageData data;
    public int spawnCount = 0;

    [SerializeField]
    public string CurrentRCode { get; private set; }
    private readonly string stagercode = "STG";
    [SerializeField]
    private int difficultyNum;
    [SerializeField]
    private int maxDifficultyNum;

    [SerializeField]
    private int worldNum;
    [SerializeField]
    private int maxWorldNum = 99;

    [SerializeField]
    private int stageNum;
    [SerializeField]
    private int maxStageNum = 10;



    void Start()
    {
        StageInitialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetRcode()
    {
        stgRcode = $"{stagercode}{difficultyNum}{worldNum}{stageNum}";
    }

    public void StageInitialize()
    {
        ReturnPools();
        //data = DataManager.Instance.GetData<StageData>(stgRcode);
        //UI초기화
        InitStageUI();
        SummonMonster();
    }

    private void InitStageUI()
    {

    }

    private void SummonMonster()
    {
        data = DataManager.Instance.GetData<StageData>(stgRcode);
        StartCoroutine(spawner.RandGroupCount(data));
    }

    private void ReturnPools()
    {

    }
    #region 다음 스테이지
    public void ToNextStage(int index)
    {
        if(stageNum <= maxStageNum)
            stageNum += index;
        else
            ToNextWorld(); 
        SetRcode();
        StageInitialize();
    }

    private void ToNextWorld()
    {
        stageNum = 1;
        if (worldNum <= maxWorldNum)
            worldNum++;
        else
            ToNextdifficulty();
    }

    private void ToNextdifficulty()
    {
        difficultyNum++;
    }
    #endregion
}
