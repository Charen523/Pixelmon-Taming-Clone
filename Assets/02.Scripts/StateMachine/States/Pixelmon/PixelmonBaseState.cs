using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelmonBaseState : BaseState
{
    protected PixelmonStateMachine pixelmonStateMachine;
    public PixelmonBaseState(PixelmonStateMachine stateMachine) : base(stateMachine)
    {
        pixelmonStateMachine = stateMachine;
    }

    public override void Enter()
    {

    }

    public override void Execute()
    {

    }

    public override void Exit()
    {

    }
}
