using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFailState : FailState
{
    private PlayerStateMachine playerStateMachine;
    public PlayerFailState(PlayerStateMachine stateMachine)
        : base(stateMachine)
    {
        playerStateMachine = stateMachine;
    }

    public override void Enter() 
    { 
        base.Enter();
        GameManager.Instance.OnStageTimeOut -= playerStateMachine.stageTimeOut;
    }
}
