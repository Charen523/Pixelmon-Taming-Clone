using System;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public EnemyData data;
    
    #region Enemy States
    public EnemyIdleState IdleState { get; private set; }
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
        //MovementSpeed = data.spd;
        //AttackRange = data.atkRange;

        MovementSpeed = 1.3f;
        AttackRange = 2f;

        ChaseState = new EnemyChaseState(this);
        AttackState = new EnemyAttackState(this);
        DieState = new EnemyDieState(this);

        GameManager.Instance.OnPlayerDie += () => ChangeState(IdleState);
        GameManager.Instance.OnStageTimeOut += () => ChangeState(IdleState);
        ChaseState.OnTargetReached += () => ChangeState(AttackState);

        ChangeState(ChaseState);
    }

    public void OnEnemyDead()
    {
        //public void OnMonsterDead(string rcode, GameObject enemy)
    }

    public void OnEnemyAttack()
    {
        //HealthSystem
    }
}