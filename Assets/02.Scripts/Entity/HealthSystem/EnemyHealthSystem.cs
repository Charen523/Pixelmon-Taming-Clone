using System;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    [SerializeField] protected Enemy enemy;

    private void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponent<Enemy>();

            if (enemy == null)
            {
                Debug.LogError($"{gameObject.name} 객체에 Enemy 클래스 없음!");
            }
        }
    }

    public void initEnemyHealth(float hp)
    {
        maxHealth = hp;
        currentHealth = maxHealth;
    }

    public override void TakeDamage(float delta, bool isCri = false, bool isPlayer = false)
    {
        def = enemy.statHandler.enemyDef;
        base.TakeDamage(delta, isCri, isPlayer);
    }

    protected override void NoticeDead()
    {
        enemy.fsm.ChangeState(enemy.fsm.DieState);
    }
}
