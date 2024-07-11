using System;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    [SerializeField] private Enemy enemy;

    private void OnEnable()
    {
        maxHealth = enemy.data.hp;
        currentHealth = maxHealth;
    }

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


    protected override void NoticeDead()
    {
        enemy.fsm.ChangeState(enemy.fsm.DieState);
    }
}
