using System;

[Serializable]
public class EnemyData : IData
{
    public string rcode;
    public string name;
    public string spawnWorldId;
    public bool isBoss;
    public float atk;
    public float hp;
    public float def;
    public float atkRange;
    public float spd;
    public float atkSpd;
    public float cri;
    public float criDmg;
    public string rewardTypes;
    public string rewardValues;
    public int troopCount;
    
    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}