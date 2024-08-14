using System;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum questType
{
    normalMonster,
    bossMonster,
    stage,
    egg,
    upgradeAtk,
    skill,
    feed,
    seed,
    harvest,
    goldDungeon
}

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
    [SerializeField] private GameObject questClear;
    [SerializeField] private TextMeshProUGUI rewardTxt;
    [SerializeField] private Image rwdIcon;
    [SerializeField] private Sprite[] rwdSprite;
    #endregion

    [SerializeField] private int maxQNum = 4;

    public bool isMobQ => curQIndex == "Q1" || curQIndex == "Q4";
    public bool isHatchQ => curQIndex == "Q2";
    public bool isBossQ => curQIndex == "Q3";
    //public bool isStageQ => curQIndex == "Q4";

    private int StageProgress => int.Parse(userData.curStage);
    
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
        SetIconSprite();

        if (curQIndex == "Q2")
        {
            string result = Calculater.NumFormatter(curQData.rewardValue * (curRepeat + 1));
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

        if (IsQuestClear())
        {
            questClear.SetActive(true);
        }
    }

    private void SetIconSprite()
    {
        switch (curQIndex)
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
            if (curQIndex == "Q2")
            {
                RewardManager.Instance.SpawnRewards(curQData.rewardType, curQData.rewardValue * (curRepeat + 1));
            }
            else
            {
                RewardManager.Instance.SpawnRewards(curQData.rewardType, curQData.rewardValue);
            }

            questClear.SetActive(false);
            SetNewQuestId();
            ResetProgress();
            SetQuestUI();
            Firebase.Analytics.FirebaseAnalytics.LogEvent($"repeat cycle:{curRepeat}, QuestNum:{curQIndex}");
        }
        else
        {
            UIManager.Instance.ShowWarn("퀘스트 조건이 충족되지 않았습니다!!");
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