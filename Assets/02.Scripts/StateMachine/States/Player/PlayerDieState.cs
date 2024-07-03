public class PlayerDieState : DieState
{
    PlayerStateMachine PlayerStateMachine;
    public PlayerDieState(PlayerStateMachine stateMachine) 
        : base(stateMachine)
    {
        PlayerStateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.NotifyPlayerDie();
    }
}
