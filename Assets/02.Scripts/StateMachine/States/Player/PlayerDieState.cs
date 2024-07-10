public class PlayerDieState : DieState
{
    PlayerStateMachine PlayerStateMachine;
    public PlayerDieState(PlayerStateMachine stateMachine) 
        : base(stateMachine)
    {
        PlayerStateMachine = stateMachine;
    }

    public override void Exit() 
    { 
        base.Exit();
        GameManager.Instance.NotifyPlayerDie();
    }
}
