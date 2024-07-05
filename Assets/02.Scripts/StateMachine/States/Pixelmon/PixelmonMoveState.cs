using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PixelmonMoveState : PixelmonBaseState
{
    
    public PixelmonMoveState(PixelmonStateMachine stateMachine) : base(stateMachine)
    {
        
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animationData.MoveParameterHash);
    }


    public override void Execute()
    {
        
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animationData.MoveParameterHash);
    }
}
