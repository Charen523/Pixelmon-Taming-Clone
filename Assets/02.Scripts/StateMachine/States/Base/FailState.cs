using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailState : BaseState
{
    public FailState(StateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animData.FailParameterHash);
    }

    public override void Execute()
    {
    }

    public override void FixedExecute()
    {
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animData.FailParameterHash);
    }
}
