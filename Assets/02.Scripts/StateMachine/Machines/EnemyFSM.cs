using System;
using UnityEngine;

public class EnemyFSM : FSM
{
    public Enemy enemy;
    public GameObject enemyCollision;

    #region Enemy States
    public IdleState IdleState { get; private set; }
    public EnemyChaseState ChaseState {  get; private set; }
    public EnemyAttackState AttackState { get; private set; }
    public EnemyDieState DieState { get; private set; }
    #endregion

    private void OnEnable()
    {
        ChangeState(ChaseState);
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

        target = Player.Instance.HitPosition;
    }
    public void Init()
    {
        IdleState = new IdleState(this);
        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        ChangeState(ChaseState);
    }

    public void OnEnemyAttack()
    {
        Player.Instance.healthSystem.TakeDamage(enemy.statHandler.GetDamage());
    }

    public void OnEnemyDead()
    {
        gameObject.SetActive(false);
        StageManager.Instance.MonsterDead(enemy.statHandler.data, gameObject);
    }
}