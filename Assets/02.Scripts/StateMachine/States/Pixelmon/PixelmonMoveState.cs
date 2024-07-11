using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PixelmonMoveState : MoveState
{
    private new PixelmonFSM fsm;
    public PixelmonMoveState(PixelmonFSM fsm) 
        : base(fsm)
    {
        this.fsm = fsm;
    }

    public override void Execute()
    {
        base.Execute();
    }
}
