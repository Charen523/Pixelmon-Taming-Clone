using System;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    [SerializeField] private Enemy enemy;
    
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

        maxHealth = enemy.data.hp;
        currentHealth = maxHealth;
    }


    protected override void NoticeDead()
    {
        enemy.stateMachine.ChangeState(enemy.stateMachine.DieState);
    }
}
