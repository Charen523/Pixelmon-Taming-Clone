using System;
using System.Collections.Generic;

[Serializable]
public class StageData : IData
{
    public string rcode;

    public int difficulty;
    public int worldId;
    public int stageId;
    public int spawnCount;
    public int nextStageCount;
    public List<string> monsterIds;
    public List<string> bossId;
    public List<string> rewardTypes;
    public List<string> rewardValues;
    public List<string> offlineRewardTypes;
    public List<string> offlineRewardValues;

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}