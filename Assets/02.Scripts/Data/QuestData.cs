using System;

[Serializable]
public class QuestData : IData
{
    public string rcode;
    public string description;
    public int goal;
    public string rewardType;
    public int rewardValue;

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}
