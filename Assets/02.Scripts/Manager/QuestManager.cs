using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum QuestType
{
    Default,
    UserLv,
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

    private QuestData data;
    private int questNum;
    private string mainQIndex;
    private int repeatCount;
    private int repeatQIndex = -1;
    private readonly string maxMainQNum = "Q15";
    private readonly int maxRepeatNum = 4;

    private QuestType curType;
    private int curGoal;
    private int curProgress;
    private int curRwd;

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
        Firebase.Analytics.FirebaseAnalytics.LogEvent($"Start_QuestIndex_{questNum}");
    }
    
    #region UI
    private void SetQuestUI()
    {
        SetQuestNameTxt();
        SetQuestCountTxt();
        SetRwdUI();
    }

    private void SetQuestNameTxt()
    {
        string curDescription = data.description;

        if (curType == QuestType.Stage)
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
        questNameTxt.text = $"{questNum}. " + curDescription;
    }

    private void SetQuestCountTxt()
    {
        int goal = curGoal;
        int progress = Mathf.Min(curProgress, goal);

        if (curType == QuestType.Stage)
        {
            if (IsQuestClear())
            {
                progress = 1;
                goal = 1;
            }
            else
            {
                progress = 0;
                goal = 1;
            }
        }
        
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

    private void SetRwdUI()
    {
        switch (data.rewardType)
        {
            case "RWD_Gold":
                rwdIcon.sprite = rwdSprite[0];
                break;
            case "RWD_Dia":
                rwdIcon.sprite = rwdSprite[1];
                break;
            case "RWD_Egg":
                rwdIcon.sprite = rwdSprite[2];
                break;
            case "RWD_Seed":
                rwdIcon.sprite = rwdSprite[3];
                break;
            case "RWD_Food":
                rwdIcon.sprite = rwdSprite[4];
                break;
            case "RWD_KeyA":
                rwdIcon.sprite = rwdSprite[5];
                break;
            case "RWD_Skill":
                rwdIcon.sprite = rwdSprite[6];
                break;
        }
        rewardTxt.text = data.rewardValue.ToString();
        curRwd = data.rewardValue;
    }

    public void QuestClearBtn()
    {
        if (IsQuestClear())
        {
            Firebase.Analytics.FirebaseAnalytics.LogEvent($"QuestIndex_{questNum}");
            RewardManager.Instance.SpawnRewards(data.rewardType, curRwd);
            questClear.SetActive(false);
            SetQuestIndex();
            ResetProgress();
            SetQuestUI();
        }
        else
        {
            UIManager.Instance.ShowWarn("퀘스트 조건이 충족되지 않았습니다!!");
        }
    }
    #endregion
    
    private void GetQuestIndex()
    {
        string[] splitId = userData.questIndex.Split('_');
        repeatCount = int.Parse(splitId[0]);
        if (int.TryParse(splitId[1], out int index))
        {
            repeatQIndex = index;
            questNum = (repeatCount - 1) * maxRepeatNum + repeatQIndex;
            data = DataManager.Instance.GetData<QuestData>(repeatQIndex.ToString());
        }
        else
        {
            mainQIndex = splitId[1];
            questNum = int.Parse(mainQIndex.Substring(1, mainQIndex.Length - 1));
            data = DataManager.Instance.GetData<QuestData>(mainQIndex);
        }

        curType = data.type;
        curGoal = data.goal;
    }

    private void SetQuestIndex()
    {
        string qNum;

        if (repeatCount == 0)
        {
            if (mainQIndex == maxMainQNum)
            {//마지막 메인Q
                repeatCount++;
                qNum = "01";
            }
            else
            {//메인Q 진행중
                int index = int.Parse(mainQIndex.Substring(1, mainQIndex.Length - 1));
                qNum = "Q" + (index + 1).ToString();
            }
        }
        else if (repeatQIndex == maxRepeatNum)
        {//마지막 반복Q
            repeatCount++;
            repeatQIndex = 1;
            qNum = repeatQIndex.ToString();
        }
        else
        {//반복Q 진행중
            repeatQIndex++;
            qNum = repeatQIndex.ToString("D2");
            
        }

        data = DataManager.Instance.GetData<QuestData>(qNum);
        string newId = repeatCount.ToString() + "_" + qNum;
        saveManager.SetData(nameof(userData.questIndex), newId);

        curType = data.type;
        curGoal = data.goal;
    }


    public bool IsMyTurn(QuestType type)
    {
        try
        {
            return type == curType;
        }
        catch
        {
            curType = data.type;
            return type == curType;
        }
    }

    #region Quest Progress
    private void ResetProgress()
    {
        int progress;

        switch (curType)
        {
            case QuestType.UserLv:
                progress = userData.userLv;
                break;
            case QuestType.Stage:
                progress = int.Parse(userData.curStage);
                break;
            case QuestType.UpgradeAtk:
                progress = userData.UpgradeLvs[0];
                break;
            default:
                progress = 0;
                break;
        }
        saveManager.SetData(nameof(userData.questProgress), progress);
        curProgress = progress;
        SetQuestCountTxt();
    }

    private void UpdateProgress()
    {
        int progress = curProgress;
        switch (curType)
        {
            case QuestType.UserLv:
                progress = userData.userLv;
                break;
            case QuestType.Stage:
                progress = int.Parse(userData.curStage);
                break;
            case QuestType.UpgradeAtk:
                progress = userData.UpgradeLvs[0];
                break;
            default:
                progress++;
                break;
        }
        saveManager.SetData(nameof(userData.questProgress), progress);
        curProgress = progress;
        SetQuestCountTxt();
    }

    private bool IsQuestClear()
    {
        if (curType == QuestType.Stage)
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
}