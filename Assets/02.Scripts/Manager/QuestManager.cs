using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum QuestType
{
    Default,
    Mob,
    Boss,
    Stage,
    Egg,
    UpgradeAtk,
    Skill,
    Feed,
    Seed,
    Harvest,
    GoldDg
}

public class QuestManager : Singleton<QuestManager>
{
    public event Action QuestEvent;

    private StageManager stageManager;
    private SaveManager saveManager;
    private UserData userData;

    private QuestData curQData;
    private QuestType questType;
    private string questId;
    private string mainQIndex;
    private int repeatCount;
    private int repeatQIndex;
    private readonly string maxMainQNum = "Q15";
    private readonly int maxRepeatNum = 4;

    private int curGoal => CurQuestGoal();
    private int curProgress => userData.questProgress;
    
    #region UI
    [SerializeField] private TextMeshProUGUI questNameTxt;
    [SerializeField] private TextMeshProUGUI countTxt;
    [SerializeField] private GameObject questClear;
    [SerializeField] private TextMeshProUGUI rewardTxt;
    [SerializeField] private Image rwdIcon;
    [SerializeField] private Sprite[] rwdSprite;
    #endregion

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
        GetQuestIndex();
        SetQuestUI();
        curQData = DataManager.Instance.GetData<QuestData>(mainQIndex);
    }
    
    #region UI
    private void SetQuestUI()
    {
        SetQuestNameTxt();
        SetQuestCountTxt();
        SetIconSprite();

        if (mainQIndex == "Q2")
        {
            string result = Calculater.NumFormatter(curQData.rewardValue * (repeatCount + 1));
            rewardTxt.text = result;
        }
        else
        {
            rewardTxt.text = curQData.rewardValue.ToString();
        }
    }

    private void SetQuestNameTxt()
    {
        string curDescription = curQData.description;

        //if (isStageQ)
        //{
        //    int diff = curGoal / 10000;
        //    string diffString = stageManager.SetDiffTxt(diff);
        //    curDescription = curDescription.Replace("D", diffString);

        //    int world = (curGoal / 100) % 100;
        //    curDescription = curDescription.Replace("W", world.ToString());

        //    int stage = curGoal % 100;
        //    curDescription = curDescription.Replace("S", stage.ToString());
        //}
        curDescription = curDescription.Replace("N", curGoal.ToString());

        questNameTxt.text = curDescription;
    }

    private void SetQuestCountTxt()
    {
        int goal = curGoal;
        int progress = curProgress;
        progress = Mathf.Min(curProgress, goal);
        countTxt.text = $"({progress} / {goal})";

        if (progress < goal)
        {
            countTxt.text = $"<color=#FF2525>({progress} / {goal})</color>";
        }
        else
        {
            countTxt.text = $"<color=#82FF55>({progress} / {goal})</color>";
            questClear.SetActive(true);
        }
    }

    private void SetIconSprite()
    {
        switch (mainQIndex)
        {
            case "Q1":
                rwdIcon.sprite = rwdSprite[0];
                break;
            case "Q2":
                rwdIcon.sprite = rwdSprite[1];
                break;
            case "Q3":
                rwdIcon.sprite = rwdSprite[2];
                break;
            case "Q4":
                rwdIcon.sprite = rwdSprite[3];
                break;
        }
    }

    public void QuestClearBtn()
    {
        if (IsQuestClear())
        {
            if (mainQIndex == "Q2")
            {
                RewardManager.Instance.SpawnRewards(curQData.rewardType, curQData.rewardValue * (repeatCount + 1));
            }
            else
            {
                RewardManager.Instance.SpawnRewards(curQData.rewardType, curQData.rewardValue);
            }

            questClear.SetActive(false);
            SetNewQuestIndex();
            ResetProgress();
            SetQuestUI();  
            Firebase.Analytics.FirebaseAnalytics.LogEvent($"repeat cycle:{repeatCount}, QuestNum:{mainQIndex}");
        }
        else
        {
            UIManager.Instance.ShowWarn("퀘스트 조건이 충족되지 않았습니다!!");
        }
    }
    #endregion

    private void SetNewQuestIndex()
    {
        string qNum;

        if (repeatCount == 0)
        {
            if (mainQIndex == maxMainQNum)
            {
                repeatCount++;
                qNum = "1";
            }
            else
            {
                int index = int.Parse(mainQIndex.Substring(1, 1));
                qNum = "Q" + (index + 1).ToString();
            }
        }
        else if (repeatQIndex == maxRepeatNum)
        {
            repeatQIndex++;
            qNum = "1";
        }
        else { qNum = repeatQIndex.ToString(); }

        string newId = repeatCount.ToString("D4") + qNum;
        SaveManager.Instance.SetData(nameof(SaveManager.Instance.userData.questId), newId);
        curQData = DataManager.Instance.GetData<QuestData>(mainQIndex);
    }

    private void GetQuestIndex()
    {
        questId = userData.questId;
        repeatCount = int.Parse(questId.Substring(0, 4));
        if (int.TryParse(questId.Substring(4, 2), out int index))
        {
            repeatQIndex = index;
        }
        else
        {
            mainQIndex = questId.Substring(4, 2);
        }
    }

    public bool IsMyTurn(QuestType curType)
    {
        return curType == questType;
    }

    #region Quest Progress
    private void ResetProgress()
    {
        int progress = 0;

        //switch (curQIndex)
        //{
        //    case "Q1":
        //        progress = 0;
        //        break;
        //    case "Q2":
        //        progress = 0;
        //        break;
        //    case "Q3":
        //        progress = StageProgress;
        //        break;
        //    case "Q4":
        //        progress = userData.userLv;
        //        break;
        //}
        saveManager.SetData(nameof(userData.questProgress), progress);
        SetQuestCountTxt();
    }

    private void UpdateProgress()
    {
        int progress = curProgress;
        progress++;
        //switch (curQIndex)
        //{
        //    case "Q1":
        //        progress++;
        //        break;
        //    case "Q2":
        //        progress++;
        //        break;
        //    case "Q3":
        //        progress = StageProgress;
        //        break;
        //    case "Q4":
        //        progress = userData.userLv;
        //        break;
        //}
        saveManager.SetData(nameof(userData.questProgress), progress);
        SetQuestCountTxt();
    }

    private int CurQuestGoal()
    {
        int goal = curQData.goal;
        //switch (curQIndex)
        //{
        //    case "Q1":
        //        break;
        //    case "Q2":
        //        break;
        //    case "Q3":
        //        break;
        //    case "Q4":
        //        break;
        //    default:
        //        goal = -1;
        //        break;
        //}
        return goal;
    }

    private bool IsQuestClear()
    {
        return curProgress >= curGoal;
        //if (isStageQ)
        //{
        //    return curProgress > curGoal;
        //}
        //else
        //{
        //    return curProgress >= curGoal;
        //}
    }
    #endregion

    public void OnQuestEvent()
    {
        QuestEvent?.Invoke();
    }
}