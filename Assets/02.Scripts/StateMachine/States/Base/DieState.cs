using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState
{
    public DieState(StateMachine stateMachine) 
        : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animData.DieParameterHash);
    }

    public override void Execute()
    {
    }

    public override void FixedExecute()
    {
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animData.DieParameterHash);
    }
}
