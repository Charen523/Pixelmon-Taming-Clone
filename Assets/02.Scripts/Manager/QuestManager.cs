using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{

    private QuestData curQData;
    private int curGoal => curQData.goal;

    [SerializeField] private string curQuestId => SaveManager.Instance.userData.questId;
    private int curRepeat => int.Parse(curQuestId.Substring(0, 4));
    private string curQIndex => curQuestId.Substring(4, 2);
    private int curProgress => SaveManager.Instance.userData.questProgress;

    #region UI
    [SerializeField] private TextMeshProUGUI questNameTxt;
    [SerializeField] private TextMeshProUGUI countTxt;
    #endregion

    [SerializeField] private int maxQNum = 4;

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

        if (curQIndex == "Q3")
        {
            int diff = curGoal / 10000;
            string diffString = StageManager.Instance.SetDiffTxt(diff);
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
        countTxt.text = $"({curProgress} / {curGoal})";
    }

    public void QuestClearBtn()
    {
        //테스트용
        SetNewQuestId();
        InitQuest();

        //if (IsQuestClear())
        //{
        //    //TODO: 퀘스트 보상 로직

        //    //다음 조건에 맞게 초기화.
        //    SetNewQuestId();
        //    SaveManager.Instance.SetData(nameof(SaveManager.Instance.userData.questProgress), "");

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
        string newId = (curRepeat + 1).ToString("D4") + NextQrcode();
        SaveManager.Instance.SetData(nameof(SaveManager.Instance.userData.questId), newId);
    }

    private string NextQrcode()
    {
        int index = int.Parse(curQIndex.Substring(1, 1));
        if (index == maxQNum)
        {
            return "Q1";
        }
        else
        {
            return "Q" + (index + 1).ToString();
        }
    }
    #endregion

    #region
    private void SetProgress()
    {

    }

    private bool IsQuestClear()
    {
        if (true)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}