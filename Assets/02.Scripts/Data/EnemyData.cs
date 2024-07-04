using System;

[Serializable]
public class EnemyData : IData
{
    public string rcode;

    public float attackRange;
    public int damage;
    public float baseSpeed;
    
    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}