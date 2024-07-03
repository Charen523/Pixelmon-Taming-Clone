using System;

[Serializable]
public class ItemData : IData
{
    public string Rcode;

    string IData.Rcode => Rcode;  // 명시적 인터페이스 구현
}