using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyStatHandler : MonoBehaviour
{
    [SerializeField]private Enemy enemy;
    public EnemyData data;

    #region Enemy Status
    public float[] enemyStats = new float[6];
    #endregion

    private void Awake()
    {
        data = DataManager.Instance.GetData<EnemyData>(transform.parent.name);
        enemyStats[0] = data.spd;
        enemyStats[1] = data.atk;
        enemyStats[2] = data.hp;
        enemyStats[3] = data.def;
        enemyStats[4] = data.cri;
        enemyStats[5] = data.criDmg;
    }

    private void OnEnable()
    {
        int difficulty = SaveManager.Instance.userData.curDifficulty;

        int deltaDifficulty = (int)Mathf.Pow(2, difficulty);

        for (int i = 1; i < enemyStats.Length; i++)
        {
            enemyStats[i] = enemyStats[i] * deltaDifficulty;
        }
        enemy.healthSystem.initEnemyHealth(enemyStats[(int)EnemyStat.MaxHp]);
    }

    public float GetDamage()
    {
        int cri = (int)enemyStats[(int)EnemyStat.Cri];
        int isCri = Random.Range(1, 101);

        if (isCri > cri) //크리x
        {
            return enemyStats[(int)EnemyStat.Atk];
        }
        else //크리
        {
            return enemyStats[(int)EnemyStat.CriDmg];
        }
    }
}
