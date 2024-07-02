using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(BaseStateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animationData.IdleParameterHash);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animationData.IdleParameterHash);
    }
}
