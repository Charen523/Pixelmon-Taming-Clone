using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : MoveState
{
    public PlayerMoveState(StateMachine stateMachine, Transform target)
        : base(stateMachine, target)
    {
    }

    public override void Execute()
    {
        // 플레이어 입력에 따라 이동

        // 입력이 없을 경우 타겟을 향해 이동
        base.Execute();
    }
}
