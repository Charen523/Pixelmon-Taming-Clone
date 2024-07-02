
[System.Serializable]
public class EnemyData
{
    public string Rcode;

    #region Attack Data
    public float attackRange;
    public int damage;
    public float dealCoolTime;
    #endregion

    #region Movement Data
    public float baseSpeed;
    public float baseRotationDamping;
    #endregion
}