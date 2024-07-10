using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PixelmonMoveState : MoveState
{
    public PixelmonMoveState(PixelmonStateMachine stateMachine) 
        : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.Instance.OnPlayerMove -= Player.Instance.playerMove;
    }

    public override void Execute()
    {
        Flip();
    }
}
