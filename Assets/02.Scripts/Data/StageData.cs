using System;
using System.Collections.Generic;

[Serializable]
public class StageData : IData
{
    public string rcode;

    #region json keys
    public int difficulty;
    public int worldId;
    public int stageId;
    public int spawnCount;
    public int nextStageCount;
    public string monsterIds;
    public float bossTimeCount;
    public string rewardTypes;
    public string rewardValues;
    public string offlineRewardTypes;
    public string offlineRewardValues;
    #endregion

    #region converted arrays
    public string[] monsterId;
    public string[] rewardType;
    public int[] rewardValue;
    public string[] offlineRewardType;
    public int[] offlineRewardValue;
    #endregion

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현

    public void ParseData()
    {
        monsterId = monsterIds.Split(" ");
        rewardType = rewardTypes.Split(" ");
        rewardValue = Array.ConvertAll(rewardValues.Split(" "), int.Parse);
        offlineRewardType = offlineRewardTypes.Split(" ");
        offlineRewardValue = Array.ConvertAll(offlineRewardValues.Split(" "), int.Parse);
    }
}