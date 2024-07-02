
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTime = 0;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.movementSpeedModifier = 0;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Execute()
    {
        base.Execute();

        if (IsDead())
        {
            stateMachine.ChangeState(stateMachine.DieState);
            return;
        }
        else if (!IsAttackRange())
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        if (attackTime <= 0)
        {
            attackTime = 1f;
            Attack();
        }

        attackTime -= Time.deltaTime;
    }

    private void Attack()
    {

    }
}