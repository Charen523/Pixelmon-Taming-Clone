using System;
using TMPro;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public event Action<string> QuestEvent;

    StageManager stageManager;
    SaveManager saveManager;
    UserData userData;

    private QuestData curQData => DataManager.Instance.GetData<QuestData>(curQIndex);
    private int curGoal => CurQuestGoal();

    [SerializeField] private string curQuestId => userData.questId;
    private int curRepeat => int.Parse(curQuestId.Substring(0, 4));
    public string curQIndex => curQuestId.Substring(4, 2);
    private int curProgress => userData.questProgress;

    #region UI
    [SerializeField] private TextMeshProUGUI questNameTxt;
    [SerializeField] private TextMeshProUGUI countTxt;
    #endregion

    [SerializeField] private int maxQNum = 4;

    public bool isMonsterQ => curQIndex == "Q2";
    public bool isStageQ => curQIndex == "Q3";
    public bool isHatchQ => curQIndex == "Q4";

    protected override void Awake()
    {
        isDontDestroyOnLoad = false;
        base.Awake();

        stageManager = StageManager.Instance;
        saveManager = SaveManager.Instance;
        userData = SaveManager.Instance.userData;
    }

    private void Start()
    {
        QuestEvent += UpdateProgress;
        InitQuest();
    }

    #region UI
    private void InitQuest()
    {
        SetQuestNameTxt();
        SetQuestCountTxt();
    }

    private void SetQuestNameTxt()
    {
        string curDescription = curQData.description;

        if (isStageQ)
        {
            int diff = curGoal / 10000;
            string diffString = stageManager.SetDiffTxt(diff);
            curDescription = curDescription.Replace("D", diffString);

            int world = (curGoal / 100) % 100;
            curDescription = curDescription.Replace("W", world.ToString());

            int stage = curGoal % 100;
            curDescription = curDescription.Replace("S", stage.ToString());
        }
        else
        {
            curDescription = curDescription.Replace("N", curGoal.ToString());
        }

        questNameTxt.text = curDescription;
    }

    private void SetQuestCountTxt()
    {
        int goal = curGoal;
        int progress = curProgress;
        if (isStageQ)
        {
            progress = progress >= goal ? 1 : 0;
            goal = 1;
        }
        countTxt.text = $"({progress} / {goal})";
    }

    public void QuestClearBtn()
    {
        SetNewQuestId();
        ResetProgress();
        InitQuest();


        //if (IsQuestClear())
        //{
        //    //TODO: 퀘스트 보상 로직

        //    //다음 조건에 맞게 초기화.
        //    SetNewQuestId();
        //    saveManager.SetData(nameof(userData.questProgress), "");

        //    //UI set.
        //    InitQuest();
    }
    #endregion

    #region Quest ID
    private void SetNewQuestId()
    {
        int repeatNum = curRepeat;
        int index = int.Parse(curQIndex.Substring(1, 1));
        string qNum = "Q" + (index + 1).ToString();

        if (index == maxQNum)
        {
            repeatNum++;
            qNum = "Q1";
        }

        string newId = repeatNum.ToString("D4") + qNum;
        SaveManager.Instance.SetData(nameof(SaveManager.Instance.userData.questId), newId);
    }
    #endregion

    #region Quest Progress
    private void ResetProgress()
    {
        int data;

        switch (curQIndex)
        {
            case "Q1":
                data = userData.userLv;
                break;
            case "Q2":
                data = 0;
                break;
            case "Q3":
                 data = stageManager.diffNum * 10000 + stageManager.worldNum * 100 + stageManager.stageNum;
                break;
            case "Q4":
                data = 0;
                break;
            default:
                data = -1;
                Debug.LogWarning("퀘스트 rcode 범위를 넘었습니다.");
                break;
        }

        saveManager.SetData(nameof(userData.questProgress), data);
    }

    private void UpdateProgress(string qNum)
    {
        if (isStageQ)
        {
            int progress = stageManager.diffNum * 10000 + stageManager.worldNum * 100 + stageManager.stageNum;

        }
        else
        {

        }
    }

    private int CurQuestGoal()
    {
        int goal = curQData.goal;

        switch (curQIndex)
        {
            case "Q1":
                goal = curRepeat + 2;
                break;
            case "Q2":
                goal = ((curRepeat / 10) + 1) * 10;
                break;
            case "Q3":
                goal = (curRepeat / 5) * 10000 + (2 + 2 * (curRepeat % 5)) * 100 + 1;
                break;
            case "Q4":
                goal = ((curRepeat / 10) + 1) * 10;
                break;
            default:
                goal = -1;
                Debug.LogWarning("퀘스트 rcode 범위를 넘었습니다.");
                break;
        }

        return goal;
    }
    #endregion


    public void OnQuestEvent(string qNum)
    {
        QuestEvent?.Invoke(qNum);
    }
}