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

    private void Awake()
    {
        data = DataManager.Instance.GetData<EnemyData>(transform.parent.name);
        enemyAtk = data.atk;
        enemyMaxHp = data.hp;
        enemyDef = data.def;
        enemyCri = data.cri;
        enemyCriDmg = data.criDmg;
    }

    private void OnEnable()
    {
        int difficulty = SaveManager.Instance.userData.curDifficulty;

        int deltaDifficulty = (int)Mathf.Pow(2, difficulty);

        enemyAtk = data.atk * deltaDifficulty;
        enemyMaxHp = data.hp * deltaDifficulty;
        enemyDef = data.def * deltaDifficulty;
        enemyCri = data.cri * deltaDifficulty;
        enemyCriDmg = data.criDmg * deltaDifficulty;

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
