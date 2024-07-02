
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
}