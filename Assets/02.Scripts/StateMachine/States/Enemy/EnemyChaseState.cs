
public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        fsm.movementSpeedModifier = 1;
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
        }
        else if (IsAttackRange())
        {
            fsm.ChangeState(fsm.AttackState);
        }
    }
}