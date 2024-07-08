using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PixelmonMoveState : MoveState
{
    private PixelmonStateMachine pixelmonStateMachine;
    private Transform enemyTarget;
    public PixelmonMoveState(PixelmonStateMachine stateMachine, Transform target) 
        : base(stateMachine, target)
    {
        pixelmonStateMachine = stateMachine;
        enemyTarget = target;
    }

    public override void Execute()
    {
        //if (enemyTarget.position.x - stateMachine.transform.position.x > 0)
        //{
        //    stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        //}
        //else
        //{
        //    stateMachine.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        //}
        base.Execute();
    }
}
