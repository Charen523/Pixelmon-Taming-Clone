using System;

[Serializable]
public class PixelmonData : IData
{
    public string rcode;
    public string name;
    public int rank;
    public float atk;
    public float sAtk;
    public float cri;
    public float criDmg;
    public float atkDmg;
    public float hit;

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}
