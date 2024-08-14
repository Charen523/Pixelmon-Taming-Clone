using System;

[Serializable]
public class QuestData : IData
{
    public string rcode;
    public questType type;
    public string description;
    public int goal;
    public string rewardType;
    public int rewardValue;

    string IData.Rcode => rcode;
}