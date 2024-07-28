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

    private void OnEnable()
    {
        UpdateEnemyStats();
    }

    private void UpdateEnemyStats()
    {
        //TODO: 받아오는 방식 변경: 위치는 Spawner or StageManager.
        data = DataManager.Instance.GetData<EnemyData>(transform.parent.name);

        int difficulty = SaveManager.Instance.userData.curDifficulty;
        int deltaDiff = (int)Mathf.Pow(2, difficulty);

        enemyAtk = data.atk * deltaDiff;
        enemyMaxHp = data.hp * deltaDiff;
        enemyDef = data.def * deltaDiff;
        enemyCri = data.cri * deltaDiff;
        enemyCriDmg = data.criDmg * deltaDiff;

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
