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
