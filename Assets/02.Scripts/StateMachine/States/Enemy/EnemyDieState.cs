
public class EnemyDieState : EnemyBaseState
{
    public EnemyDieState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        fsm.movementSpeedModifier = 0;
        base.Enter();
    }

    public override void Execute()
    {
        base.Execute();
        //애니메이션 재생 끝나면 소멸.
    }
}