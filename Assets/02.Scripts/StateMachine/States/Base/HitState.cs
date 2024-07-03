using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : BaseState
{
    public HitState(StateMachine stateMachine) 
        : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animationData.HitParameterName);
    }

    public override void Execute()
    {

    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animationData.HitParameterName);
    }
}
