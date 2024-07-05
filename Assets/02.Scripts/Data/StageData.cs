using System;

[Serializable]
public class StageData : IData
{
    public string Rcode;
    
    public int World_Id;
    public int Stage_Id;
    public int Spawn_Count;
    public int Normal_Id;
    string IData.Rcode => Rcode;  // 명시적 인터페이스 구현
}