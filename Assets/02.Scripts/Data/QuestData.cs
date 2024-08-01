using System;

[Serializable]
public class QuestData : IData
{
    public string rcode;
    public string description;
    public string goal;
    public string rewardTypes;
    public string rewardValues;

    public string[] rewardType => rewardTypes.Split(' ');
    public int[] rewardValue => Array.ConvertAll(rewardValues.Split(' '), int.Parse);

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}
