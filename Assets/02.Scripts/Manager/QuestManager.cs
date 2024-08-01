using TMPro;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    StageManager stageManager;
    SaveManager saveManager;
    UserData userData;

    private QuestData curQData;
    private int curGoal => CurQuestGoal();

    [SerializeField] private string curQuestId => userData.questId;
    private int curRepeat => int.Parse(curQuestId.Substring(0, 4));
    private string curQIndex => curQuestId.Substring(4, 2);
    private int curProgress => userData.questProgress;

    #region UI
    [SerializeField] private TextMeshProUGUI questNameTxt;
    [SerializeField] private TextMeshProUGUI countTxt;
    #endregion

    [SerializeField] private int maxQNum = 4;
    private bool isStageQ => curQIndex == "Q3";

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
        InitQuest();
    }

    #region UI
    private void InitQuest()
    {
        curQData = DataManager.Instance.GetData<QuestData>(curQIndex);
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
        if (isStageQ)
        {
            goal = 1;
        }
        countTxt.text = $"({curProgress} / {goal})";
    }

    public void QuestClearBtn()
    {
        //테스트용
        SetNewQuestId();
        //reset init data
        InitQuest();

        //if (IsQuestClear())
        //{
        //    //TODO: 퀘스트 보상 로직

        //    //다음 조건에 맞게 초기화.
        //    SetNewQuestId();
        //    saveManager.SetData(nameof(userData.questProgress), "");

        //    //UI set.
        //    InitQuest();
        //}
        //else
        //{
        //    Debug.Log("퀘스트 조건 충족하지 못함");
        //}
    }
    #endregion

    #region Quest ID
    private void SetNewQuestId()
    {
        string repeat;
        string qRcode;

        int index = int.Parse(curQIndex.Substring(1, 1));
        if (index == maxQNum)
        {
            repeat = (curRepeat + 1).ToString("D4");
            qRcode = "Q1";
        }
        else
        {
            repeat = curRepeat.ToString("D4");
            qRcode = "Q" + (index + 1).ToString();
        }

        string newId = repeat + qRcode;
        saveManager.SetData(nameof(userData.questId), newId);
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
                data = 0;
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

    private bool ChangeQuestProgress()
    {
        bool isClear = false;

        if (isStageQ)
        {
            int progress = stageManager.diffNum * 10000 + stageManager.worldNum * 100 + stageManager.stageNum;

        }
        else
        {
             
        }

        if (isClear)
        {
            //Tab 켜져 있으면 팝업창 띄우기 -> 퀘스트 완료버튼 A
            //아니면 퀘스트 완료 UI. -> 퀘스트 완료 버튼 B
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}