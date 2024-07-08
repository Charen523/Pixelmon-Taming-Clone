using System;
using UnityEngine;

public class EnemyHealthSystem : HealthSystem
{
    private EnemyData enemyData;

    private void Start()
    {
        enemyData = gameObject.GetComponent<EnemyStateMachine>().data;

        // MaxHealth 값 초기화 및 업데이트 람다식 할당
        MaxHealth = enemyData.hp;
        Action updateMaxHealth = () => MaxHealth = enemyData.hp;

        // MaxHealth 값이 변경될 때마다 실행되는 람다식 할당
        Action onMaxHealthChanged = () =>
        {
            if (currentHealth > MaxHealth)
            {
                currentHealth = MaxHealth;
            }
        };

    }

    public override bool ChangeHealth(float damage)
    {
        throw new System.NotImplementedException();
    }

    protected override void NoticeDead()
    {
        throw new System.NotImplementedException();
    }
}