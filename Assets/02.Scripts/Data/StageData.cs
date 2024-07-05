using System;

[Serializable]
public class StageData : IData
{
    public string rcode;

    public int difficulty;
    public int worldId;
    public int stageId;
    public int spawnCount;
    public int nextStageCount;
    public string monsterIds;
    public string bossId;
    public string rewardTypes;
    public string rewardValues;
    public string offlineRewardTypes;
    public string offlineRewardValues;

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}