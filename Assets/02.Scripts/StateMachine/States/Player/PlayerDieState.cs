using UnityEngine;

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
        fsm.joystick.gameObject.SetActive(false);
        fsm.rb.velocity = Vector2.zero;
        GameManager.Instance.NotifyPlayerDie();
    }
}
