
public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.movementSpeedModifier = 1;
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (IsDead())
        {
            stateMachine.ChangeState(stateMachine.DieState);
        }
        else if (IsAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
    }

    protected bool IsDead()
    {
        //애니메이션 GetBool로 죽음 상태 가져오기??
        return false;
    }

    protected bool IsAttackRange()
    {
        float playerDistanceSqr = (stateMachine.target.transform.position - stateMachine.Enemy.transform.position).sqrMagnitude;
        return playerDistanceSqr <= stateMachine.Enemy.attackRange * stateMachine.Enemy.attackRange;
    }
}
}