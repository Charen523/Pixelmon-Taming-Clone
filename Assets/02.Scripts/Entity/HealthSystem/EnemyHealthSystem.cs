using System;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    [SerializeField] protected Enemy enemy;

    [SerializeField] private Transform fillBar;

    private void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponent<Enemy>();
        }
    }

    protected override void Update()
    {
        fillBar.localScale = new Vector3(currentHealth / maxHealth, 1, 1);
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
