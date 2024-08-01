using TMPro;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    [SerializeField] private string curQuestId = "R000_Q0";
    
    #region UI
    [SerializeField] private TextMeshProUGUI questNameTxt;
    [SerializeField] private TextMeshProUGUI countTxt;
    #endregion

    private QuestData curQData;

    [SerializeField] private int maxQNum = 4;

    private string curQStatus;

    protected override void Awake()
    {
        base.Awake();
        curQuestId = SaveManager.Instance.userData.questId;
    }

    private void SetQuestNameTxt()
    {

    }

    private string InsertValues (string description, string goal)
    {
        string[] goals = goal.Split ('_');

        foreach (string g in goals)
        {
            description = description.Replace("N", g);
        }

        return description; 
    }

    private void SetQuestCountTxt()
    {
        int status;
        int goal = 1;

        if (!int.TryParse(curQData.goal, out goal))
        {
            status = (curQStatus == curQData.goal) ? 1 : 0; //클리어 판독기로 변경.
        }
        else
        {
            status = int.Parse(curQStatus);
        }

        countTxt.text = $"({status} / {goal})";
    }

    private void CurDescription()
    {

    }
}