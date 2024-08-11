using UnityEngine;

public class EnemyStatHandler : MonoBehaviour
{
    [SerializeField]private Enemy enemy;
    public EnemyData data;

    #region Enemy Status
    public float enemySpd;
    public float enemyAtk;
    public float enemyMaxHp;
    public float enemyDef;
    public float enemyCri;
    public float enemyCriDmg;
    #endregion

    public void UpdateEnemyStats()
    {
        int difficulty = StageManager.Instance.diffNum;
        int deltaDiff = difficulty + 1;

        int world = StageManager.Instance.worldNum;
        int stage = StageManager.Instance.stageNum;

        float deltaStage = (world - 1) * 20 + stage;


        enemyAtk = data.atk * (deltaStage / 100) * deltaDiff;
        enemyMaxHp = data.hp * (deltaStage / 100) * deltaDiff;
        enemyDef = data.def * (deltaStage / 100) * deltaDiff;

        enemy.healthSystem.initEnemyHealth(enemyMaxHp);
    }

    public float GetDamage()
    {
        int isCri = Random.Range(1, 101);

        if (isCri > enemyCri) //크리x
        {
            return enemyAtk;
        }
        else //크리
        {
            return enemyCriDmg;
        }
    }
}
