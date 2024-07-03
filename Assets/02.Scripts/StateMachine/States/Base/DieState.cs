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
        StartAnimation(stateMachine.animationData.DieParameterName);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animationData.DieParameterName);
    }
}
