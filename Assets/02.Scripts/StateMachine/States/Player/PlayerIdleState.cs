using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IdleState
{
    private PlayerStateMachine playerStateMachine;
    public PlayerIdleState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        playerStateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.OnStageClear -= playerStateMachine.stageClear;
        playerStateMachine.MoveState.OnTargetReached -= playerStateMachine.targetReached;
    }
}
