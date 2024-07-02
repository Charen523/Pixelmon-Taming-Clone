
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTime = 0;

    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        fsm.movementSpeedModifier = 0;
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
            fsm.ChangeState(fsm.DieState);
            return;
        }
        else if (!IsAttackRange())
        {
            fsm.ChangeState(fsm.ChaseState);
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