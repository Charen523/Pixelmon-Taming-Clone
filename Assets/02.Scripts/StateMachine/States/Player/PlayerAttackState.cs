using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : IdleState
{
    private new PlayerFSM fsm;
    public PlayerAttackState(PlayerFSM fsm)
        : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Enter()
    {
        base.Enter();
        Player.Instance.ChangePixelmonsState(PixelmonState.Attack);
    }
}
