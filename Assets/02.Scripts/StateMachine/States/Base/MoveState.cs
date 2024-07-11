using System;
using Unity.VisualScripting;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(FSM fsm) 
        : base(fsm)
    {
    }

    public override void Enter()
    {
        StartAnimation(fsm.animData.MoveParameterHash);
    }

    public override void Execute()
    {
        Flip();
    }

    public override void Exit()
    {
        StopAnimation(fsm.animData.MoveParameterHash);        
    }

    protected void Flip()
    {
        if (fsm.target != null)
        {
            float targetPositionX = fsm.target.transform.position.x;
            float currentPositionX = fsm.transform.position.x;

            bool shouldFlip = targetPositionX > currentPositionX;
            fsm.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = shouldFlip;
        }
    }

    protected void MoveTowardsTarget(float moveSpeed, float attackRange, IState newState)
    {
        // 자동으로 타겟에게 이동하는 로직
        if (fsm.rb != null && fsm.target.transform != null)
        {
            Vector2 currentPosition = fsm.rb.position;
            Vector2 targetPosition = fsm.target.transform.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;
            fsm.rb.velocity = direction * moveSpeed;

            // 타겟에게 도착하면 공격 상태로 변경
            if (Vector2.Distance(currentPosition, targetPosition) < attackRange)
            {
                fsm.rb.velocity = Vector3.zero;
                fsm.ChangeState(newState);
            }
        }
    }
}
