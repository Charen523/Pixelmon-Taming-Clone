using System.Numerics;
using UnityEngine;

public class CheatManager : Singleton<CheatManager>
{
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            switch(Input.inputString)
            {
                case "s":
                case "S":
                    NextStage();
                    break;
                case "g":
                case "G":
                    BigInteger money = 100000000;
                    SaveManager.Instance.SetFieldData("gold", money, true);
                    break;
                case "d":
                case "D":
                    SaveManager.Instance.SetFieldData(nameof(SaveManager.Instance.userData.diamond), 10000, true);
                    break;
                case "q":
                case "Q":
                    QuestCheat();
                    break;
                default:
                    break;
            }
        }
    }

    private void NextStage()
    {
        StageManager.Instance.killCount = StageManager.Instance.data.nextStageCount;
    }

    private void QuestCheat()
    {
        RewardManager.Instance.SpawnRewards(QuestManager.Instance.data.rewardType, QuestManager.Instance.curRwd);
        QuestManager.Instance.questClear.SetActive(false);
        QuestManager.Instance.SetQuestIndex();
        QuestManager.Instance.ResetProgress();
        QuestManager.Instance.SetQuestUI();
    }
#endif
}