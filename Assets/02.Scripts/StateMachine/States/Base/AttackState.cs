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
        StartAnimation(stateMachine.animData.AttackParameterHash);
    }

    public override void Execute()
    {
    }

    public override void FixedExecute()
    {
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animData.AttackParameterHash);
    }
}
