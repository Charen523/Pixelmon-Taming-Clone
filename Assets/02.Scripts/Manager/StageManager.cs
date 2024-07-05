using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    public string CurrentRCode {  get; private set; }

    [SerializeField]
    private string stgRcode;
    [SerializeField]
    StageData data;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetRcode()
    {
        stgRcode = $"STG{difficultyNum}{worldNum}{stageNum}";
    }

    public void StageInitialize()
    {
        data = DataManager.Instance.GetData<StageData>(stgRcode);
        //UI초기화
    }

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
}
