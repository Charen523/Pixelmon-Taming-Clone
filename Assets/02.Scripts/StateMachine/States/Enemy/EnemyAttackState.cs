
using UnityEngine;

public class EnemyAttackState : AttackState
{
    private float attackTime = 0;

    public EnemyAttackState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
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

    }
}