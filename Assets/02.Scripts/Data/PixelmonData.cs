using System;

[Serializable]
public class PixelmonData : IData
{
    public string rcode;

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}
