public class PlayerDieState : DieState
{
    private new PlayerFSM fsm;
    public PlayerDieState(PlayerFSM fsm) 
        : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Enter()
    {
        base.Enter();
        Player.Instance.ChangePixelmonsState(PixelmonState.Idle);
    }

    public override void Exit() 
    { 
        base.Exit();
        GameManager.Instance.NotifyPlayerDie(); 
    }
}
