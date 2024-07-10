using System;

[Serializable]
public class EnemyData : IData
{
    #region json keys
    public string rcode;
    public string name;
    public string spawnWorldId;
    public bool isBoss;
    public float atk;
    public float hp;
    public float def;
    public float spd;
    public string rewardTypes;
    public string rewardRates;
    public string rewardValues;
    #endregion

    #region converted arrays
    public string[] rewardType => rewardTypes.Split(" ");
    public float[] rewardRate => Array.ConvertAll(rewardRates.Split(" "), float.Parse);
    public int[] rewardValue => Array.ConvertAll(rewardValues.Split(" "), int.Parse);
    #endregion

    public float dmg;

    string IData.Rcode => rcode;  // 명시적 인터페이스 구현
}