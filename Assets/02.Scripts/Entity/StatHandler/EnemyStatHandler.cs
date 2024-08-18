using System.Numerics;
using UnityEngine;

public class EnemyStatHandler : MonoBehaviour
{
    [SerializeField]private Enemy enemy;
    public EnemyData data;

    #region Enemy Status
    public BigInteger enemyAtk;
    public BigInteger enemyMaxHp;
    public BigInteger enemyDef;
    #endregion

    public void UpdateEnemyStats()
    {
        int difficulty = StageManager.Instance.diffNum;
        int deltaDiff = difficulty + 1;

        int world = StageManager.Instance.worldNum;
        int stage = StageManager.Instance.stageNum;

        int deltaStage = (world - 1) * 20 + stage;

        if (data.isBoss)
        {
            enemyAtk =  data.Atk * (deltaStage * 5 + 100)  * deltaDiff / 100;
            enemyMaxHp = data.Hp * (deltaStage * 20 + 100)  * deltaDiff / 100;
            enemyDef = data.Def * (deltaStage * 5 + 100) * deltaDiff / 100;
        }
        else
        {
            enemyAtk = data.Atk * (deltaStage * 5 + 100) * deltaDiff / 100;
            enemyMaxHp = data.Hp * (deltaStage * 10 + 100) * deltaDiff / 100;
            enemyDef = data.Def * (deltaStage * 5 + 100) * deltaDiff / 100;
        }
        enemy.healthSystem.initEnemyHealth(enemyMaxHp);
    }

    public BigInteger GetDamage()
    {
        return enemyAtk;
    }
}
