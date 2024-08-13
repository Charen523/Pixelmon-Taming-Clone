using UnityEngine;

public class EnemyStatHandler : MonoBehaviour
{
    [SerializeField]private Enemy enemy;
    public EnemyData data;

    #region Enemy Status
    public float enemyAtk;
    public float enemyMaxHp;
    public float enemyDef;
    #endregion

    public void UpdateEnemyStats()
    {
        int difficulty = StageManager.Instance.diffNum;
        int deltaDiff = difficulty + 1;

        int world = StageManager.Instance.worldNum;
        int stage = StageManager.Instance.stageNum;

        float deltaStage = (world - 1) * 20 + stage;

        if (data.isBoss)
        {
            enemyAtk = data.atk * ((deltaStage * 5 + 100) * 0.01f) * deltaDiff;
            enemyMaxHp = data.hp * ((deltaStage * 20 + 100) * 0.01f) * deltaDiff;
            enemyDef = data.def * ((deltaStage * 5 + 100) * 0.01f) * deltaDiff;
        }
        else
        {
            enemyAtk = data.atk * ((deltaStage * 5 + 100) * 0.01f) * deltaDiff;
            enemyMaxHp = data.hp * ((deltaStage * 10 + 100) * 0.01f) * deltaDiff;
            enemyDef = data.def * ((deltaStage * 5 + 100) * 0.01f) * deltaDiff;
        }
        enemy.healthSystem.initEnemyHealth(enemyMaxHp);
    }

    public float GetDamage()
    {
        return enemyAtk;
    }
}
