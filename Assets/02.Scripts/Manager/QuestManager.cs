using System;
using TMPro;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public event Action QuestEvent;

    private StageManager stageManager;
    private SaveManager saveManager;
    private UserData userData;

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

    public bool isHatchQ => curQIndex == "Q1";
    public bool isMonsterQ => curQIndex == "Q2";
    public bool isStageQ => curQIndex == "Q3";
    public bool isUserLevelQ => curQIndex == "Q4";

    private int StageProgress => stageManager.diffNum * 10000 + stageManager.worldNum * 100 + stageManager.stageNum;
    

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
            countTxt.text = $"<color=#FF2525>({progress} / {goal})</color>";
        }
        else
        {
            countTxt.text = $"<color=#82FF55>({progress} / {goal})</color>";
        }

    }

    public void QuestClearBtn()
    {
        if (IsQuestClear())
        {
            SetNewQuestId();
            ResetProgress();
            SetQuestUI();
        }
        else
        {
            ShowWarn("퀘스트 조건이 충족되지 않았습니다!!");
        }
    }
    #endregion

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

    #region Quest Progress
    private void ResetProgress()
    {
        int progress = 0;

        switch (curQIndex)
        {
            case "Q2":
            case "Q4":
                break;
            case "Q1":
                progress = userData.userLv;
                break;
            case "Q3":
                 progress = StageProgress;
                break;
        }

        saveManager.SetData(nameof(userData.questProgress), progress);
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
                progress = userData.userLv;
                break;
        }

        saveManager.SetData(nameof(userData.questProgress), progress);
        SetQuestCountTxt();
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
                break;
        }

        return goal;
    }

    private bool IsQuestClear()
    {
        if (isStageQ)
        {
            return curProgress > curGoal;
        }
        else
        {
            return curProgress >= curGoal;
        }
    }
    #endregion

    public void OnQuestEvent()
    {
        QuestEvent?.Invoke();
    }

    public async void ShowWarn(string msg)
    {
        await UIManager.Show<WarnPopup>(msg);
    }
}