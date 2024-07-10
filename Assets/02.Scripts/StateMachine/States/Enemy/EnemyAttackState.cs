
using UnityEngine;

public class EnemyAttackState : AttackState
{
    EnemyStateMachine enemyStateMachine;
    private float attackTime = 0;

    public EnemyAttackState(EnemyStateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        enemyStateMachine.ChaseState.OnTargetReached -= enemyStateMachine.targetReached;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Execute()
    {
        base.Execute();

        //if (IsDead())
        //{
        //    fsm.ChangeState(fsm.DieState);
        //    return;
        //}
        //else if (!IsAttackRange())
        //{
        //    fsm.ChangeState(fsm.ChaseState);
        //    return;
        //}

        if (attackTime <= 0)
        {
            attackTime = 1f;
            Attack();
        }

        attackTime -= Time.deltaTime;
    }

    private void Attack()
    {
        enemyStateMachine.OnEnemyAttack();
    }
}