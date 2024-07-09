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
        //if (enemyTarget.position.x - stateMachine.transform.position.x > 0)
        //{
        //    stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        //}
        //else
        //{
        //    stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        //}
    }
}
