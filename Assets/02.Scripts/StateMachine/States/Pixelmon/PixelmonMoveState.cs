using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PixelmonMoveState : MoveState
{
    private PixelmonStateMachine pixelmonStateMachine;
    public PixelmonMoveState(PixelmonStateMachine stateMachine, Transform target) 
        : base(stateMachine, target)
    {
        pixelmonStateMachine = stateMachine;
    }

    public override void FixedExecute()
    {
        Flip();
    }
}
