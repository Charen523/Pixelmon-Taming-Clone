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
        //if (position.x - stateMachine.transform.position.x > 0)
        //{
        //    stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        //}
        //else
        //{
        //    stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        //}

        base.Execute();
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animationData.MoveParameterHash);
    }
}
