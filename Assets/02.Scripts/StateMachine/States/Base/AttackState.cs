using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public AttackState(StateMachine stateMachine) 
        : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animationData.AttackParameterHash);
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animationData.AttackParameterHash);
    }
}
