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
    public string[] monsterId => monsterIds.Split(' ');
    public string[] rewardType => rewardTypes.Split(' ');
    public int[] rewardValue => Array.ConvertAll(rewardValues.Split(' '), int.Parse);
    public string[] offlineRewardType => offlineRewardTypes.Split(' ');
    public int[] offlineRewardValue => Array.ConvertAll(offlineRewardValues.Split(' '), int.Parse);
    #endregion

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}