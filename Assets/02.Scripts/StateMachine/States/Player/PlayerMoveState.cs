using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveState : MoveState
{
    private new PlayerFSM fsm;
    public PlayerMoveState(PlayerFSM fsm)
        :base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Enter()
    {
        base.Enter();
        Player.Instance.ChangePixelmonsState(PixelmonState.Move);
    }

    public override void Execute()
    {
        base.Execute();
        // 플레이어 입력에 따라 이동
        if (fsm.isActiveMove)
        {
            fsm.rb.velocity = fsm.MovementInput * Player.Instance.data.spd;
        }
        // 입력이 없을 경우 타겟을 향해 이동
        else
        {
            MoveTowardsTarget(Player.Instance.data.spd, Player.Instance.data.atkRange, fsm.AttackState);
        }
    }
}
