using System;

[Serializable]
public class StageData : IData
{
    public string rcode;
    
    public int World_Id;
    public int Stage_Id;
    
    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}