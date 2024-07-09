using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(StateMachine stateMachine)
        : base(stateMachine)
    {
    }

    public override void Enter()
    {
        StartAnimation(stateMachine.animData.IdleParameterHash);
    }

    public override void Execute()
    {
        // 탐색로직
        // 몬스터 찾음 -> moveState로 변경
    }

    public override void Exit()
    {
        StopAnimation(stateMachine.animData.IdleParameterHash);
    }
}
