using System;
using TMPro;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public event Action QuestEvent;

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

    public bool isUserLevelQ => curQIndex == "Q1";
    public bool isMonsterQ => curQIndex == "Q2";
    public bool isStageQ => curQIndex == "Q3";
    public bool isHatchQ => curQIndex == "Q4";

    private int StageProgress => stageManager.diffNum * 10000 + stageManager.worldNum * 100 + stageManager.stageNum;
    private bool isQuestClear => curProgress >= curGoal;

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
        SetQuestUI();
    }
    
    #region UI
    private void SetQuestUI()
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
        int progress = Mathf.Min(curProgress, goal); 
        if (isStageQ)
        {
            progress = progress >= goal ? 1 : 0;
            goal = 1;
        }

        if (progress < goal)
        {
            countTxt.text = $"<color=#82FF55>({progress} / {goal})</color>";
        }
        else
        {
            countTxt.text = $"<color=#82FF55>({progress} / {goal})</color>";
        }

    }

    public void QuestClearBtn()
    {
        if (isQuestClear)
        {
            SetNewQuestId();
            ResetProgress();
            SetQuestUI();

        }
        else
        {
            Debug.LogWarning("퀘스트 미완료");
        }
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
        int progress;

        switch (curQIndex)
        {
            case "Q1":
                progress = userData.userLv;
                break;
            case "Q2":
                progress = 0;
                break;
            case "Q3":
                 progress = StageProgress;
                break;
            case "Q4":
                progress = curGoal; //TODO: 알 부화에 이벤트 걸기
                break;
            default:
                progress = -1;
                Debug.LogWarning("퀘스트 rcode 범위를 넘었습니다.");
                break;
        }

        saveManager.SetData(nameof(userData.questProgress), progress);
        if (isQuestClear)
        {
            //TODO: 퀘스트 클리어 시 UI 변동주기
        }
    }

    private void UpdateProgress()
    {
        int progress = curProgress;

        switch (curQIndex)
        {
            case "Q1":
                progress = userData.userLv;
                break;
            case "Q2":
                progress++;
                break;
            case "Q3":
                progress = StageProgress;
                break;
            case "Q4":
                progress++;
                break;
            default:
                Debug.LogWarning("퀘스트 rcode 범위를 넘었습니다.");
                break;
        }

        saveManager.SetData(nameof(userData.questProgress), progress);
        SetQuestCountTxt();

        if (isQuestClear)
        {
            //TODO: 퀘스트 클리어 시 UI 변동주기
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

    public void OnQuestEvent()
    {
        QuestEvent?.Invoke();
    }
}